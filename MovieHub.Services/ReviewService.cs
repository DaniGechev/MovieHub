using Microsoft.EntityFrameworkCore;

using MovieHub.Data;
using MovieHub.Data.Models;
using MovieHub.Services.Contracts;
using MovieHub.Services.Models.Reviews;

namespace MovieHub.Services
{
    public class ReviewService : IReviewService
    {
        private readonly ApplicationDbContext data;

        public ReviewService(ApplicationDbContext data)
        {
            this.data = data;
        }

        public async Task<IEnumerable<ReviewServiceModel>> GetForMovieAsync(int movieId)
        {
            return await this.data.Reviews
                .Where(r => r.MovieId == movieId)
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
                .ToListAsync();
        }

        public async Task<ReviewServiceModel?> AddAsync(
            int movieId, string authorId, string content, int rating)
        {
            var movieExists = await this.data.Movies.AnyAsync(m => m.Id == movieId);

            if (!movieExists)
            {
                return null;
            }

            var review = new Review
            {
                MovieId = movieId,
                AuthorId = authorId,
                Content = content,
                Rating = rating,
                CreatedOn = DateTime.UtcNow,
            };

            await this.data.Reviews.AddAsync(review);
            await this.data.SaveChangesAsync();

            var authorName = await this.data.Users
                .Where(u => u.Id == authorId)
                .Select(u => u.NickName)
                .FirstOrDefaultAsync();

            return new ReviewServiceModel
            {
                Id = review.Id,
                Content = review.Content,
                Rating = review.Rating,
                CreatedOn = review.CreatedOn,
                AuthorName = authorName ?? "Unknown",
                MovieId = review.MovieId,
            };
        }

        public async Task<bool> DeleteAsync(int reviewId, string userId, bool isAdmin)
        {
            var review = await this.data.Reviews.FindAsync(reviewId);

            if (review == null)
            {
                return false;
            }

            // Only the original author or an administrator may delete a review.
            if (review.AuthorId != userId && !isAdmin)
            {
                return false;
            }

            this.data.Reviews.Remove(review);
            await this.data.SaveChangesAsync();

            return true;
        }
    }
}
