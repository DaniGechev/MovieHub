using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using MovieHub.Services.Contracts;
using MovieHub.Services.Models.Movies;
using MovieHub.Web.Infrastructure;
using MovieHub.Web.Models.Movies;

using static MovieHub.Web.Infrastructure.GlobalConstants;

namespace MovieHub.Web.Controllers
{
    public class MoviesController : Controller
    {
        private readonly IMovieService movieService;
        private readonly IGenreService genreService;
        private readonly IFavoriteService favoriteService;

        public MoviesController(
            IMovieService movieService,
            IGenreService genreService,
            IFavoriteService favoriteService)
        {
            this.movieService = movieService;
            this.genreService = genreService;
            this.favoriteService = favoriteService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index([FromQuery] AllMoviesQueryModel query)
        {
            var result = await this.movieService.GetAllAsync(
                query.SearchTerm,
                query.Sorting,
                query.CurrentPage,
                AllMoviesQueryModel.MoviesPerPage);

            query.Movies = result.Movies;
            query.TotalMoviesCount = result.TotalMoviesCount;

            return View(query);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var movie = await this.movieService.GetDetailsAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            var isFavorite = false;
            if (this.User.Identity?.IsAuthenticated == true)
            {
                isFavorite = await this.favoriteService.IsFavoriteAsync(id, this.User.Id());
            }

            return View(new MovieDetailsViewModel
            {
                Movie = movie,
                IsFavorite = isFavorite,
            });
        }

        [Authorize(Roles = AdministratorRoleName)]
        public async Task<IActionResult> Create()
        {
            return View(new MovieFormModel
            {
                Genres = await this.GetGenresAsync(),
            });
        }

        [HttpPost]
        [Authorize(Roles = AdministratorRoleName)]
        public async Task<IActionResult> Create(MovieFormModel model)
        {
            if (!await this.genreService.ExistsAsync(model.GenreId))
            {
                ModelState.AddModelError(nameof(model.GenreId), "That genre does not exist.");
            }

            if (!ModelState.IsValid)
            {
                model.Genres = await this.GetGenresAsync();
                return View(model);
            }

            var id = await this.movieService.CreateAsync(ToServiceModel(model));

            return RedirectToAction(nameof(Details), new { id });
        }

        [Authorize(Roles = AdministratorRoleName)]
        public async Task<IActionResult> Edit(int id)
        {
            var movie = await this.movieService.GetForEditAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            return View(new MovieFormModel
            {
                Title = movie.Title,
                Description = movie.Description,
                ReleaseYear = movie.ReleaseYear,
                Director = movie.Director,
                ImageUrl = movie.ImageUrl,
                GenreId = movie.GenreId,
                Genres = await this.GetGenresAsync(),
            });
        }

        [HttpPost]
        [Authorize(Roles = AdministratorRoleName)]
        public async Task<IActionResult> Edit(int id, MovieFormModel model)
        {
            if (!await this.genreService.ExistsAsync(model.GenreId))
            {
                ModelState.AddModelError(nameof(model.GenreId), "That genre does not exist.");
            }

            if (!ModelState.IsValid)
            {
                model.Genres = await this.GetGenresAsync();
                return View(model);
            }

            var updated = await this.movieService.EditAsync(id, ToServiceModel(model));

            if (!updated)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        [Authorize(Roles = AdministratorRoleName)]
        public async Task<IActionResult> Delete(int id)
        {
            await this.movieService.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }

        private static MovieFormServiceModel ToServiceModel(MovieFormModel model)
            => new()
            {
                Title = model.Title,
                Description = model.Description,
                ReleaseYear = model.ReleaseYear,
                Director = model.Director,
                ImageUrl = model.ImageUrl,
                GenreId = model.GenreId,
            };

        private async Task<IEnumerable<MovieGenreViewModel>> GetGenresAsync()
        {
            var genres = await this.genreService.GetAllAsync();

            return genres.Select(g => new MovieGenreViewModel
            {
                Id = g.Id,
                Name = g.Name,
            });
        }
    }
}
