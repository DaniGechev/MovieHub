using MovieHub.Data.Models;
using MovieHub.Services;

namespace MovieHub.Tests.Services
{
    [TestFixture]
    public class GenreServiceTests : TestBase
    {
        private GenreService service = null!;

        [SetUp]
        public void Init()
        {
            this.service = new GenreService(this.data);
        }

        [Test]
        public async Task CreateAsync_AddsGenre_AndReturnsId()
        {
            var id = await this.service.CreateAsync("Horror");

            Assert.That(id, Is.GreaterThan(0));
            Assert.That(this.data.Genres.Single().Name, Is.EqualTo("Horror"));
        }

        [Test]
        public async Task GetAllAsync_ReturnsGenresOrderedByName()
        {
            this.data.Genres.AddRange(
                new Genre { Id = 1, Name = "Western" },
                new Genre { Id = 2, Name = "Action" });
            this.data.SaveChanges();

            var result = (await this.service.GetAllAsync()).ToList();

            Assert.That(result, Has.Count.EqualTo(2));
            Assert.That(result.First().Name, Is.EqualTo("Action"));
        }

        [Test]
        public async Task GetByIdAsync_ReturnsGenre_WhenExists()
        {
            this.data.Genres.Add(new Genre { Id = 1, Name = "Drama" });
            this.data.SaveChanges();

            var result = await this.service.GetByIdAsync(1);

            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Name, Is.EqualTo("Drama"));
        }

        [Test]
        public async Task GetByIdAsync_ReturnsNull_WhenMissing()
        {
            var result = await this.service.GetByIdAsync(99);

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task EditAsync_UpdatesName_AndReturnsTrue()
        {
            this.data.Genres.Add(new Genre { Id = 1, Name = "Comdy" });
            this.data.SaveChanges();

            var updated = await this.service.EditAsync(1, "Comedy");

            Assert.That(updated, Is.True);
            Assert.That(this.data.Genres.Single().Name, Is.EqualTo("Comedy"));
        }

        [Test]
        public async Task EditAsync_ReturnsFalse_WhenMissing()
        {
            var updated = await this.service.EditAsync(50, "Anything");

            Assert.That(updated, Is.False);
        }

        [Test]
        public async Task DeleteAsync_RemovesGenre_AndReturnsTrue()
        {
            this.data.Genres.Add(new Genre { Id = 1, Name = "Temp" });
            this.data.SaveChanges();

            var deleted = await this.service.DeleteAsync(1);

            Assert.That(deleted, Is.True);
            Assert.That(this.data.Genres.Any(), Is.False);
        }

        [Test]
        public async Task DeleteAsync_ReturnsFalse_WhenMissing()
        {
            var deleted = await this.service.DeleteAsync(7);

            Assert.That(deleted, Is.False);
        }

        [Test]
        public async Task ExistsAsync_ReturnsCorrectResult()
        {
            this.data.Genres.Add(new Genre { Id = 1, Name = "Exists" });
            this.data.SaveChanges();

            Assert.That(await this.service.ExistsAsync(1), Is.True);
            Assert.That(await this.service.ExistsAsync(2), Is.False);
        }
    }
}
