using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using MovieHub.Services.Contracts;
using MovieHub.Web.Infrastructure;

namespace MovieHub.Web.Controllers
{
    [Authorize]
    public class FavoritesController : Controller
    {
        private readonly IFavoriteService favoriteService;

        public FavoritesController(IFavoriteService favoriteService)
        {
            this.favoriteService = favoriteService;
        }

        // Called via fetch from the movie details page.
        [HttpPost]
        public async Task<IActionResult> Toggle(int movieId)
        {
            var isFavorite = await this.favoriteService.ToggleAsync(movieId, this.User.Id());

            return Json(new { isFavorite });
        }

        public async Task<IActionResult> Mine()
        {
            var movies = await this.favoriteService.GetUserFavoritesAsync(this.User.Id());

            return View(movies);
        }
    }
}
