using MovieHub.Services.Models.Movies;

namespace MovieHub.Services.Contracts
{
    public interface IFavoriteService
    {
        /// <summary>
        /// Adds the movie to the user's favorites if absent, removes it if present.
        /// Returns true when the movie is now a favorite, false when it was removed.
        /// </summary>
        Task<bool> ToggleAsync(int movieId, string userId);

        Task<bool> IsFavoriteAsync(int movieId, string userId);

        Task<IEnumerable<MovieListServiceModel>> GetUserFavoritesAsync(string userId);
    }
}
