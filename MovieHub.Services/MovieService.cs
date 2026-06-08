using Microsoft.EntityFrameworkCore;

using MovieHub.Data;
using MovieHub.Data.Models;
using MovieHub.Services.Contracts;
using MovieHub.Services.Models.Movies;
using MovieHub.Services.Models.Reviews;

namespace MovieHub.Services
{
    public class MovieService : IMovieService
    {
        private readonly ApplicationDbContext data;

        public MovieService(ApplicationDbContext data)
        {
            this.data = data;
        }

        public async Task<MovieQueryServiceModel> GetAllAsync(
            string? searchTerm = null,
            MovieSorting sorting = MovieSorting.Newest,
            int currentPage = 1,
            int moviesPerPage = 6)
        {
            var query = this.data.Movies.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var term = searchTerm.ToLower();
                query = query.Where(m =>
                    m.Title.ToLower().Contains(term) ||
                    m.Director.ToLower().Contains(term));
            }

            query = sorting switch
            {
                MovieSorting.Oldest => query.OrderBy(m => m.ReleaseYear),
                MovieSorting.TitleAlphabetical => query.OrderBy(m => m.Title),
                MovieSorting.HighestRated => query
                    .OrderByDescending(m => m.Reviews.Average(r => (double?)r.Rating) ?? 0),
                _ => query.OrderByDescending(m => m.ReleaseYear),
            };

            var totalCount = await query.CountAsync();

            var movies = await query
                .Skip((currentPage - 1) * moviesPerPage)
                .Take(moviesPerPage)
                .Select(m => new MovieListServiceModel
                {
                    Id = m.Id,
                    Title = m.Title,
                    ImageUrl = m.ImageUrl,
                    ReleaseYear = m.ReleaseYear,
                    GenreName = m.Genre.Name,
                })
                .ToListAsync();

            return new MovieQueryServiceModel
            {
                TotalMoviesCount = totalCount,
                Movies = movies,
            };
        }

        public async Task<MovieDetailsServiceModel?> GetDetailsAsync(int id)
        {
            return await this.data.Movies
                .Where(m => m.Id == id)
                .Select(m => new MovieDetailsServiceModel
                {
                    Id = m.Id,
                    Title = m.Title,
                    Description = m.Description,
                    ReleaseYear = m.ReleaseYear,
                    Director = m.Director,
                    ImageUrl = m.ImageUrl,
                    GenreId = m.GenreId,
                    GenreName = m.Genre.Name,
                    AverageRating = m.Reviews.Average(r => (double?)r.Rating) ?? 0,
                    Reviews = m.Reviews
                        .OrderByDescending(r => r.CreatedOn)
                        .Select(r => new ReviewServiceModel
                        {
                            Id = r.Id,
                            Content = r.Content,
                            Rating = r.Rating,
                            CreatedOn = r.CreatedOn,
                            AuthorName = r.Author.NickName,
                            MovieId = r.MovieId,
                        })
                        .ToList(),
                })
                .FirstOrDefaultAsync();
        }

        public async Task<MovieFormServiceModel?> GetForEditAsync(int id)
        {
            return await this.data.Movies
                .Where(m => m.Id == id)
                .Select(m => new MovieFormServiceModel
                {
                    Title = m.Title,
                    Description = m.Description,
                    ReleaseYear = m.ReleaseYear,
                    Director = m.Director,
                    ImageUrl = m.ImageUrl,
                    GenreId = m.GenreId,
                })
                .FirstOrDefaultAsync();
        }

        public async Task<int> CreateAsync(MovieFormServiceModel model)
        {
            var movie = new Movie
            {
                Title = model.Title,
                Description = model.Description,
                ReleaseYear = model.ReleaseYear,
                Director = model.Director,
                ImageUrl = model.ImageUrl,
                GenreId = model.GenreId,
            };

            await this.data.Movies.AddAsync(movie);
            await this.data.SaveChangesAsync();

            return movie.Id;
        }

        public async Task<bool> EditAsync(int id, MovieFormServiceModel model)
        {
            var movie = await this.data.Movies.FindAsync(id);

            if (movie == null)
            {
                return false;
            }

            movie.Title = model.Title;
            movie.Description = model.Description;
            movie.ReleaseYear = model.ReleaseYear;
            movie.Director = model.Director;
            movie.ImageUrl = model.ImageUrl;
            movie.GenreId = model.GenreId;

            await this.data.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var movie = await this.data.Movies.FindAsync(id);

            if (movie == null)
            {
                return false;
            }

            this.data.Movies.Remove(movie);
            await this.data.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ExistsAsync(int id)
            => await this.data.Movies.AnyAsync(m => m.Id == id);
    }
}
