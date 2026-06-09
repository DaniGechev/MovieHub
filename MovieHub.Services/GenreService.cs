using Microsoft.EntityFrameworkCore;

using MovieHub.Data;
using MovieHub.Data.Models;
using MovieHub.Services.Contracts;
using MovieHub.Services.Models.Genres;

namespace MovieHub.Services
{
    public class GenreService : IGenreService
    {
        private readonly ApplicationDbContext data;

        public GenreService(ApplicationDbContext data)
        {
            this.data = data;
        }

        public async Task<IEnumerable<GenreServiceModel>> GetAllAsync()
        {
            return await this.data.Genres
                .OrderBy(g => g.Name)
                .Select(g => new GenreServiceModel
                {
                    Id = g.Id,
                    Name = g.Name,
                })
                .ToListAsync();
        }

        public async Task<GenreServiceModel?> GetByIdAsync(int id)
        {
            return await this.data.Genres
                .Where(g => g.Id == id)
                .Select(g => new GenreServiceModel
                {
                    Id = g.Id,
                    Name = g.Name,
                })
                .FirstOrDefaultAsync();
        }

        public async Task<int> CreateAsync(string name)
        {
            var genre = new Genre { Name = name };

            await this.data.Genres.AddAsync(genre);
            await this.data.SaveChangesAsync();

            return genre.Id;
        }

        public async Task<bool> EditAsync(int id, string name)
        {
            var genre = await this.data.Genres.FindAsync(id);

            if (genre == null)
            {
                return false;
            }

            genre.Name = name;
            await this.data.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var genre = await this.data.Genres.FindAsync(id);

            if (genre == null)
            {
                return false;
            }

            this.data.Genres.Remove(genre);
            await this.data.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ExistsAsync(int id)
            => await this.data.Genres.AnyAsync(g => g.Id == id);
    }
}
