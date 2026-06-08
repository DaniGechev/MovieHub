namespace MovieHub.Data.Models
{
    /// <summary>
    /// Join entity that implements the many-to-many "watchlist / favorites"
    /// relationship between users and movies. The composite primary key
    /// (UserId + MovieId) is configured in ApplicationDbContext.OnModelCreating.
    /// </summary>
    public class Favorite
    {
        public string UserId { get; set; } = string.Empty;

        public ApplicationUser User { get; set; } = null!;

        public int MovieId { get; set; }

        public Movie Movie { get; set; } = null!;
    }
}
