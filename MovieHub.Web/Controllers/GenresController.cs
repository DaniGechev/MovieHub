using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using MovieHub.Services.Contracts;
using MovieHub.Web.Models.Genres;

using static MovieHub.Web.Infrastructure.GlobalConstants;

namespace MovieHub.Web.Controllers
{
    [Authorize(Roles = AdministratorRoleName)]
    public class GenresController : Controller
    {
        private readonly IGenreService genreService;

        public GenresController(IGenreService genreService)
        {
            this.genreService = genreService;
        }

        public async Task<IActionResult> Index()
        {
            var genres = await this.genreService.GetAllAsync();

            return View(genres);
        }

        public IActionResult Create()
        {
            return View(new GenreFormModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(GenreFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await this.genreService.CreateAsync(model.Name);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var genre = await this.genreService.GetByIdAsync(id);

            if (genre == null)
            {
                return NotFound();
            }

            return View(new GenreFormModel { Name = genre.Name });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, GenreFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var updated = await this.genreService.EditAsync(id, model.Name);

            if (!updated)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await this.genreService.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
