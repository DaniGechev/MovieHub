using MovieHub.Services.Models.Genres;

namespace MovieHub.Services.Contracts
{
    public interface IGenreService
    {
        Task<IEnumerable<GenreServiceModel>> GetAllAsync();

        Task<GenreServiceModel?> GetByIdAsync(int id);

        Task<int> CreateAsync(string name);

        Task<bool> EditAsync(int id, string name);

        Task<bool> DeleteAsync(int id);

        Task<bool> ExistsAsync(int id);
    }
}
