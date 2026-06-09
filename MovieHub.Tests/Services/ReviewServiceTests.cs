using MovieHub.Data.Models;
using MovieHub.Services;

namespace MovieHub.Tests.Services
{
    [TestFixture]
    public class ReviewServiceTests : TestBase
    {
        private ReviewService service = null!;

        [SetUp]
        public void Init()
        {
            this.service = new ReviewService(this.data);
        }

        private void SeedMovieAndUser()
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
            this.data.Users.Add(new ApplicationUser { Id = "u1", NickName = "Alice" });
            this.data.SaveChanges();
        }

        [Test]
        public async Task AddAsync_AddsReview_WhenMovieExists()
        {
            SeedMovieAndUser();

            var result = await this.service.AddAsync(1, "u1", "Loved it", 9);

            Assert.That(result, Is.Not.Null);
            Assert.That(result!.AuthorName, Is.EqualTo("Alice"));
            Assert.That(result.Rating, Is.EqualTo(9));
            Assert.That(this.data.Reviews.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task AddAsync_ReturnsNull_WhenMovieMissing()
        {
            SeedMovieAndUser();

            var result = await this.service.AddAsync(999, "u1", "No movie", 5);

            Assert.That(result, Is.Null);
            Assert.That(this.data.Reviews.Any(), Is.False);
        }

        [Test]
        public async Task GetForMovieAsync_ReturnsReviewsForMovie()
        {
            SeedMovieAndUser();
            this.data.Reviews.Add(new Review { Id = 1, MovieId = 1, AuthorId = "u1", Content = "A", Rating = 7, CreatedOn = DateTime.UtcNow });
            this.data.SaveChanges();

            var result = await this.service.GetForMovieAsync(1);

            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.Single().AuthorName, Is.EqualTo("Alice"));
        }

        [Test]
        public async Task DeleteAsync_RemovesReview_WhenCalledByAuthor()
        {
            SeedMovieAndUser();
            this.data.Reviews.Add(new Review { Id = 1, MovieId = 1, AuthorId = "u1", Content = "A", Rating = 7, CreatedOn = DateTime.UtcNow });
            this.data.SaveChanges();

            var deleted = await this.service.DeleteAsync(1, "u1", isAdmin: false);

            Assert.That(deleted, Is.True);
            Assert.That(this.data.Reviews.Any(), Is.False);
        }

        [Test]
        public async Task DeleteAsync_RemovesReview_WhenCalledByAdmin()
        {
            SeedMovieAndUser();
            this.data.Reviews.Add(new Review { Id = 1, MovieId = 1, AuthorId = "u1", Content = "A", Rating = 7, CreatedOn = DateTime.UtcNow });
            this.data.SaveChanges();

            var deleted = await this.service.DeleteAsync(1, "someone-else", isAdmin: true);

            Assert.That(deleted, Is.True);
        }

        [Test]
        public async Task DeleteAsync_ReturnsFalse_WhenNotAuthorAndNotAdmin()
        {
            SeedMovieAndUser();
            this.data.Reviews.Add(new Review { Id = 1, MovieId = 1, AuthorId = "u1", Content = "A", Rating = 7, CreatedOn = DateTime.UtcNow });
            this.data.SaveChanges();

            var deleted = await this.service.DeleteAsync(1, "intruder", isAdmin: false);

            Assert.That(deleted, Is.False);
            Assert.That(this.data.Reviews.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task DeleteAsync_ReturnsFalse_WhenReviewMissing()
        {
            var deleted = await this.service.DeleteAsync(123, "u1", isAdmin: true);

            Assert.That(deleted, Is.False);
        }
    }
}
