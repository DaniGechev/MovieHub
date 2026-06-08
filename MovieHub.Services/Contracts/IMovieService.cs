using MovieHub.Services.Models.Movies;

namespace MovieHub.Services.Contracts
{
    public interface IMovieService
    {
        Task<MovieQueryServiceModel> GetAllAsync(
            string? searchTerm = null,
            MovieSorting sorting = MovieSorting.Newest,
            int currentPage = 1,
            int moviesPerPage = 6);

        Task<MovieDetailsServiceModel?> GetDetailsAsync(int id);

        Task<MovieFormServiceModel?> GetForEditAsync(int id);

        Task<int> CreateAsync(MovieFormServiceModel model);

        Task<bool> EditAsync(int id, MovieFormServiceModel model);

        Task<bool> DeleteAsync(int id);

        Task<bool> ExistsAsync(int id);
    }
}
