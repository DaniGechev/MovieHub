using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using MovieHub.Services.Contracts;
using MovieHub.Web.Infrastructure;
using MovieHub.Web.Models.Reviews;

namespace MovieHub.Web.Controllers
{
    [Authorize]
    public class ReviewsController : Controller
    {
        private readonly IReviewService reviewService;

        public ReviewsController(IReviewService reviewService)
        {
            this.reviewService = reviewService;
        }

        // Called via fetch from the movie details page.
        [HttpPost]
        public async Task<IActionResult> Add(ReviewFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Please write a longer review and pick a rating from 1 to 10." });
            }

            var review = await this.reviewService.AddAsync(
                model.MovieId,
                this.User.Id(),
                model.Content,
                model.Rating);

            if (review == null)
            {
                return NotFound();
            }

            // Returned as JSON so the page can append it without reloading.
            return Json(new
            {
                review.Id,
                review.Content,
                review.Rating,
                review.AuthorName,
                CreatedOn = review.CreatedOn.ToString("dd MMM yyyy"),
            });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await this.reviewService.DeleteAsync(
                id,
                this.User.Id(),
                this.User.IsAdmin());

            if (!deleted)
            {
                return Forbid();
            }

            return Ok();
        }
    }
}
