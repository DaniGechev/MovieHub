using System.Security.Claims;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Moq;

using MovieHub.Services.Contracts;
using MovieHub.Services.Models.Genres;
using MovieHub.Services.Models.Movies;
using MovieHub.Web.Controllers;
using MovieHub.Web.Models.Movies;

namespace MovieHub.Tests.Controllers
{
    [TestFixture]
    public class MoviesControllerTests
    {
        private Mock<IMovieService> movieService = null!;
        private Mock<IGenreService> genreService = null!;
        private Mock<IFavoriteService> favoriteService = null!;
        private MoviesController controller = null!;

        [SetUp]
        public void Setup()
        {
            this.movieService = new Mock<IMovieService>();
            this.genreService = new Mock<IGenreService>();
            this.favoriteService = new Mock<IFavoriteService>();

            this.controller = new MoviesController(
                this.movieService.Object,
                this.genreService.Object,
                this.favoriteService.Object);

            // An anonymous (not signed-in) user is enough for these tests.
            this.controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity()),
                },
            };
        }

        [Test]
        public async Task Details_ReturnsNotFound_WhenMovieDoesNotExist()
        {
            this.movieService
                .Setup(s => s.GetDetailsAsync(1))
                .ReturnsAsync((MovieDetailsServiceModel?)null);

            var result = await this.controller.Details(1);

            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task Details_ReturnsViewWithModel_WhenMovieExists()
        {
            this.movieService
                .Setup(s => s.GetDetailsAsync(1))
                .ReturnsAsync(new MovieDetailsServiceModel { Id = 1, Title = "Inception" });

            var result = await this.controller.Details(1) as ViewResult;

            Assert.That(result, Is.Not.Null);
            var model = result!.Model as MovieDetailsViewModel;
            Assert.That(model, Is.Not.Null);
            Assert.That(model!.Movie.Title, Is.EqualTo("Inception"));
        }

        [Test]
        public async Task Create_Post_ReturnsView_AndDoesNotCreate_WhenModelInvalid()
        {
            this.genreService.Setup(s => s.ExistsAsync(It.IsAny<int>())).ReturnsAsync(true);
            this.genreService
                .Setup(s => s.GetAllAsync())
                .ReturnsAsync(new List<GenreServiceModel> { new() { Id = 1, Name = "Action" } });

            this.controller.ModelState.AddModelError("Title", "Required");

            var result = await this.controller.Create(new MovieFormModel { GenreId = 1 }) as ViewResult;

            Assert.That(result, Is.Not.Null);
            var model = result!.Model as MovieFormModel;
            Assert.That(model!.Genres.Count(), Is.EqualTo(1)); // dropdown repopulated
            this.movieService.Verify(
                s => s.CreateAsync(It.IsAny<MovieFormServiceModel>()),
                Times.Never);
        }

        [Test]
        public async Task Create_Post_CreatesMovie_AndRedirectsToDetails_WhenValid()
        {
            this.genreService.Setup(s => s.ExistsAsync(1)).ReturnsAsync(true);
            this.movieService
                .Setup(s => s.CreateAsync(It.IsAny<MovieFormServiceModel>()))
                .ReturnsAsync(7);

            var model = new MovieFormModel
            {
                Title = "Valid Title",
                Description = "A description long enough to pass.",
                ReleaseYear = 2000,
                Director = "Director",
                ImageUrl = "http://example.com/x.jpg",
                GenreId = 1,
            };

            var result = await this.controller.Create(model) as RedirectToActionResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result!.ActionName, Is.EqualTo("Details"));
            Assert.That(result.RouteValues!["id"], Is.EqualTo(7));
            this.movieService.Verify(
                s => s.CreateAsync(It.IsAny<MovieFormServiceModel>()),
                Times.Once);
        }
    }
}
