using MovieHub.Services.Models.Reviews;

namespace MovieHub.Services.Contracts
{
    public interface IReviewService
    {
        Task<IEnumerable<ReviewServiceModel>> GetForMovieAsync(int movieId);

        /// <summary>Adds a review. Returns null if the movie does not exist.</summary>
        Task<ReviewServiceModel?> AddAsync(int movieId, string authorId, string content, int rating);

        /// <summary>
        /// Deletes a review. Only the author or an administrator may delete it;
        /// returns false if the review is missing or the user is not allowed.
        /// </summary>
        Task<bool> DeleteAsync(int reviewId, string userId, bool isAdmin);
    }
}
