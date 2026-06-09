using Microsoft.EntityFrameworkCore;

using MovieHub.Data;

namespace MovieHub.Tests
{
    /// <summary>
    /// Gives every test its own isolated in-memory database. A unique database
    /// name per test means tests never see each other's data.
    /// </summary>
    public abstract class TestBase
    {
        protected ApplicationDbContext data = null!;

        [SetUp]
        public void SetupDatabase()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            this.data = new ApplicationDbContext(options);
        }

        [TearDown]
        public void DisposeDatabase()
        {
            this.data.Dispose();
        }
    }
}
