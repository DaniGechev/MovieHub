using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using MovieHub.Data.Models;

namespace MovieHub.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Genre> Genres { get; set; } = null!;

        public DbSet<Movie> Movies { get; set; } = null!;

        public DbSet<Review> Reviews { get; set; } = null!;

        public DbSet<Favorite> Favorites { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // IMPORTANT: configure the Identity tables first.
            base.OnModelCreating(builder);

            // ----- Composite key for the many-to-many join entity -----
            builder.Entity<Favorite>()
                .HasKey(f => new { f.UserId, f.MovieId });

            // A user can be deleted independently of their favorites being cascaded,
            // so we restrict here to avoid SQL Server's "multiple cascade paths" error.
            builder.Entity<Favorite>()
                .HasOne(f => f.User)
                .WithMany(u => u.FavoriteMovies)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Favorite>()
                .HasOne(f => f.Movie)
                .WithMany(m => m.FavoritedBy)
                .HasForeignKey(f => f.MovieId)
                .OnDelete(DeleteBehavior.Cascade);

            // ----- Review relationships -----
            // Deleting a movie removes its reviews, but deleting a user does NOT
            // cascade through the review (again, avoids multiple cascade paths).
            builder.Entity<Review>()
                .HasOne(r => r.Movie)
                .WithMany(m => m.Reviews)
                .HasForeignKey(r => r.MovieId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Review>()
                .HasOne(r => r.Author)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            // ----- Seed data (static values only, required by migrations) -----
            builder.Entity<Genre>().HasData(SeedGenres());
            builder.Entity<Movie>().HasData(SeedMovies());
        }

        private static Genre[] SeedGenres()
            => new[]
            {
                new Genre { Id = 1, Name = "Action" },
                new Genre { Id = 2, Name = "Drama" },
                new Genre { Id = 3, Name = "Science Fiction" },
                new Genre { Id = 4, Name = "Comedy" },
                new Genre { Id = 5, Name = "Thriller" },
            };

        private static Movie[] SeedMovies()
            => new[]
            {
                new Movie
                {
                    Id = 1,
                    Title = "Inception",
                    Description = "A skilled thief who steals secrets from people's dreams is given the inverse task of planting an idea into a target's mind.",
                    ReleaseYear = 2010,
                    Director = "Christopher Nolan",
                    ImageUrl = "https://placehold.co/300x450?text=Inception",
                    GenreId = 3,
                },
                new Movie
                {
                    Id = 2,
                    Title = "The Shawshank Redemption",
                    Description = "Over the years, a banker sentenced to life in prison forms an unlikely friendship and quietly works toward his own redemption.",
                    ReleaseYear = 1994,
                    Director = "Frank Darabont",
                    ImageUrl = "https://placehold.co/300x450?text=Shawshank",
                    GenreId = 2,
                },
                new Movie
                {
                    Id = 3,
                    Title = "Mad Max: Fury Road",
                    Description = "In a post-apocalyptic wasteland, a drifter and a rebel commander flee from a tyrant across the desert in a relentless chase.",
                    ReleaseYear = 2015,
                    Director = "George Miller",
                    ImageUrl = "https://placehold.co/300x450?text=Mad+Max",
                    GenreId = 1,
                },
                new Movie
                {
                    Id = 4,
                    Title = "The Grand Budapest Hotel",
                    Description = "A legendary concierge and his trusted lobby boy become entangled in the theft of a priceless painting and a family fortune dispute.",
                    ReleaseYear = 2014,
                    Director = "Wes Anderson",
                    ImageUrl = "https://placehold.co/300x450?text=Budapest+Hotel",
                    GenreId = 4,
                },
                new Movie
                {
                    Id = 5,
                    Title = "Parasite",
                    Description = "A poor family schemes to become employed by a wealthy household, but their plan unravels in unexpected and dangerous ways.",
                    ReleaseYear = 2019,
                    Director = "Bong Joon-ho",
                    ImageUrl = "https://placehold.co/300x450?text=Parasite",
                    GenreId = 5,
                },
                new Movie
                {
                    Id = 6,
                    Title = "Interstellar",
                    Description = "A team of explorers travels through a wormhole in space in a desperate attempt to ensure humanity's survival.",
                    ReleaseYear = 2014,
                    Director = "Christopher Nolan",
                    ImageUrl = "https://placehold.co/300x450?text=Interstellar",
                    GenreId = 3,
                },
            };
    }
}
