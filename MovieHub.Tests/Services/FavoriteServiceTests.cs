using MovieHub.Data.Models;
using MovieHub.Services;

namespace MovieHub.Tests.Services
{
    [TestFixture]
    public class FavoriteServiceTests : TestBase
    {
        private FavoriteService service = null!;

        [SetUp]
        public void Init()
        {
            this.service = new FavoriteService(this.data);
        }

        private void SeedMovie()
        {
            this.data.Genres.Add(new Genre { Id = 1, Name = "Action" });
            this.data.Movies.Add(new Movie
            {
                Id = 1,
                Title = "Inception",
                Description = "A long enough description.",
                ReleaseYear = 2010,
                Director = "Nolan",
                ImageUrl = "http://example.com/x.jpg",
                GenreId = 1,
            });
            this.data.SaveChanges();
        }

        [Test]
        public async Task ToggleAsync_AddsFavorite_WhenNotPresent()
        {
            SeedMovie();

            var isFavorite = await this.service.ToggleAsync(1, "u1");

            Assert.That(isFavorite, Is.True);
            Assert.That(this.data.Favorites.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task ToggleAsync_RemovesFavorite_WhenAlreadyPresent()
        {
            SeedMovie();
            this.data.Favorites.Add(new Favorite { MovieId = 1, UserId = "u1" });
            this.data.SaveChanges();

            var isFavorite = await this.service.ToggleAsync(1, "u1");

            Assert.That(isFavorite, Is.False);
            Assert.That(this.data.Favorites.Any(), Is.False);
        }

        [Test]
        public async Task IsFavoriteAsync_ReturnsCorrectResult()
        {
            SeedMovie();
            this.data.Favorites.Add(new Favorite { MovieId = 1, UserId = "u1" });
            this.data.SaveChanges();

            Assert.That(await this.service.IsFavoriteAsync(1, "u1"), Is.True);
            Assert.That(await this.service.IsFavoriteAsync(1, "other"), Is.False);
        }

        [Test]
        public async Task GetUserFavoritesAsync_ReturnsOnlyThatUsersMovies()
        {
            SeedMovie();
            this.data.Favorites.Add(new Favorite { MovieId = 1, UserId = "u1" });
            this.data.Favorites.Add(new Favorite { MovieId = 1, UserId = "u2" });
            this.data.SaveChanges();

            var result = await this.service.GetUserFavoritesAsync("u1");

            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.Single().Title, Is.EqualTo("Inception"));
        }
    }
}
