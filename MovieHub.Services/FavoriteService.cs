using Microsoft.EntityFrameworkCore;

using MovieHub.Data;
using MovieHub.Data.Models;
using MovieHub.Services.Contracts;
using MovieHub.Services.Models.Movies;

namespace MovieHub.Services
{
    public class FavoriteService : IFavoriteService
    {
        private readonly ApplicationDbContext data;

        public FavoriteService(ApplicationDbContext data)
        {
            this.data = data;
        }

        public async Task<bool> ToggleAsync(int movieId, string userId)
        {
            var favorite = await this.data.Favorites
                .FirstOrDefaultAsync(f => f.MovieId == movieId && f.UserId == userId);

            if (favorite != null)
            {
                this.data.Favorites.Remove(favorite);
                await this.data.SaveChangesAsync();

                return false; // no longer a favorite
            }

            await this.data.Favorites.AddAsync(new Favorite
            {
                MovieId = movieId,
                UserId = userId,
            });
            await this.data.SaveChangesAsync();

            return true; // now a favorite
        }

        public async Task<bool> IsFavoriteAsync(int movieId, string userId)
            => await this.data.Favorites
                .AnyAsync(f => f.MovieId == movieId && f.UserId == userId);

        public async Task<IEnumerable<MovieListServiceModel>> GetUserFavoritesAsync(string userId)
        {
            return await this.data.Favorites
                .Where(f => f.UserId == userId)
                .Select(f => new MovieListServiceModel
                {
                    Id = f.Movie.Id,
                    Title = f.Movie.Title,
                    ImageUrl = f.Movie.ImageUrl,
                    ReleaseYear = f.Movie.ReleaseYear,
                    GenreName = f.Movie.Genre.Name,
                })
                .ToListAsync();
        }
    }
}
