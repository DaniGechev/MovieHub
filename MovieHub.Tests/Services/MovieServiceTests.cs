using MovieHub.Data.Models;
using MovieHub.Services;
using MovieHub.Services.Models.Movies;

namespace MovieHub.Tests.Services
{
    [TestFixture]
    public class MovieServiceTests : TestBase
    {
        private MovieService service = null!;

        [SetUp]
        public void Init()
        {
            this.service = new MovieService(this.data);
        }

        private void SeedGenre()
        {
            this.data.Genres.Add(new Genre { Id = 1, Name = "Action" });
            this.data.SaveChanges();
        }

        private Movie SeedMovie(int id, string title, int year = 2000, string director = "Director")
        {
            var movie = new Movie
            {
                Id = id,
                Title = title,
                Description = "A long enough description for tests.",
                ReleaseYear = year,
                Director = director,
                ImageUrl = "http://example.com/poster.jpg",
                GenreId = 1,
            };

            this.data.Movies.Add(movie);
            this.data.SaveChanges();

            return movie;
        }

        [Test]
        public async Task GetAllAsync_ReturnsAllMovies_WhenNoFilter()
        {
            SeedGenre();
            SeedMovie(1, "Alpha");
            SeedMovie(2, "Beta");

            var result = await this.service.GetAllAsync();

            Assert.That(result.TotalMoviesCount, Is.EqualTo(2));
            Assert.That(result.Movies.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task GetAllAsync_FiltersBySearchTerm()
        {
            SeedGenre();
            SeedMovie(1, "Inception", director: "Nolan");
            SeedMovie(2, "Parasite", director: "Bong");

            var result = await this.service.GetAllAsync(searchTerm: "incep");

            Assert.That(result.TotalMoviesCount, Is.EqualTo(1));
            Assert.That(result.Movies.Single().Title, Is.EqualTo("Inception"));
        }

        [Test]
        public async Task GetAllAsync_SortsByTitleAlphabetical()
        {
            SeedGenre();
            SeedMovie(1, "Zodiac");
            SeedMovie(2, "Amadeus");

            var result = await this.service.GetAllAsync(sorting: MovieSorting.TitleAlphabetical);

            Assert.That(result.Movies.First().Title, Is.EqualTo("Amadeus"));
        }

        [Test]
        public async Task GetAllAsync_PagesResults()
        {
            SeedGenre();
            for (var i = 1; i <= 5; i++)
            {
                SeedMovie(i, $"Movie {i}");
            }

            var result = await this.service.GetAllAsync(currentPage: 1, moviesPerPage: 2);

            Assert.That(result.TotalMoviesCount, Is.EqualTo(5));
            Assert.That(result.Movies.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task GetDetailsAsync_ReturnsMovie_WhenExists()
        {
            SeedGenre();
            SeedMovie(1, "Inception");

            var result = await this.service.GetDetailsAsync(1);

            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Title, Is.EqualTo("Inception"));
            Assert.That(result.GenreName, Is.EqualTo("Action"));
        }

        [Test]
        public async Task GetDetailsAsync_ReturnsNull_WhenNotExists()
        {
            var result = await this.service.GetDetailsAsync(999);

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task GetDetailsAsync_ComputesAverageRating()
        {
            SeedGenre();
            SeedMovie(1, "Inception");
            this.data.Users.Add(new ApplicationUser { Id = "u1", NickName = "Alice" });
            this.data.Reviews.Add(new Review { Id = 1, MovieId = 1, AuthorId = "u1", Content = "Great", Rating = 6, CreatedOn = DateTime.UtcNow });
            this.data.Reviews.Add(new Review { Id = 2, MovieId = 1, AuthorId = "u1", Content = "Good", Rating = 8, CreatedOn = DateTime.UtcNow });
            this.data.SaveChanges();

            var result = await this.service.GetDetailsAsync(1);

            Assert.That(result!.AverageRating, Is.EqualTo(7.0));
            Assert.That(result.Reviews.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task CreateAsync_AddsMovie_AndReturnsId()
        {
            SeedGenre();

            var id = await this.service.CreateAsync(new MovieFormServiceModel
            {
                Title = "New Movie",
                Description = "A long enough description.",
                ReleaseYear = 2021,
                Director = "Someone",
                ImageUrl = "http://example.com/x.jpg",
                GenreId = 1,
            });

            Assert.That(id, Is.GreaterThan(0));
            Assert.That(this.data.Movies.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task EditAsync_UpdatesMovie_AndReturnsTrue()
        {
            SeedGenre();
            SeedMovie(1, "Old Title");

            var updated = await this.service.EditAsync(1, new MovieFormServiceModel
            {
                Title = "New Title",
                Description = "Updated description text.",
                ReleaseYear = 2022,
                Director = "Director",
                ImageUrl = "http://example.com/x.jpg",
                GenreId = 1,
            });

            Assert.That(updated, Is.True);
            Assert.That(this.data.Movies.Single().Title, Is.EqualTo("New Title"));
        }

        [Test]
        public async Task EditAsync_ReturnsFalse_WhenMovieMissing()
        {
            var updated = await this.service.EditAsync(123, new MovieFormServiceModel());

            Assert.That(updated, Is.False);
        }

        [Test]
        public async Task DeleteAsync_RemovesMovie_AndReturnsTrue()
        {
            SeedGenre();
            SeedMovie(1, "Doomed");

            var deleted = await this.service.DeleteAsync(1);

            Assert.That(deleted, Is.True);
            Assert.That(this.data.Movies.Any(), Is.False);
        }

        [Test]
        public async Task DeleteAsync_ReturnsFalse_WhenMovieMissing()
        {
            var deleted = await this.service.DeleteAsync(404);

            Assert.That(deleted, Is.False);
        }

        [Test]
        public async Task GetForEditAsync_ReturnsModel_WhenExists()
        {
            SeedGenre();
            SeedMovie(1, "Editable");

            var result = await this.service.GetForEditAsync(1);

            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Title, Is.EqualTo("Editable"));
        }

        [Test]
        public async Task ExistsAsync_ReturnsTrueAndFalse()
        {
            SeedGenre();
            SeedMovie(1, "Here");

            Assert.That(await this.service.ExistsAsync(1), Is.True);
            Assert.That(await this.service.ExistsAsync(2), Is.False);
        }
    }
}
