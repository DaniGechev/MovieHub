using MovieHub.Services.Models.Reviews;

namespace MovieHub.Services.Models.Movies
{
    public class MovieDetailsServiceModel
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int ReleaseYear { get; set; }

        public string Director { get; set; } = string.Empty;

        public string ImageUrl { get; set; } = string.Empty;

        public int GenreId { get; set; }

        public string GenreName { get; set; } = string.Empty;

        public double AverageRating { get; set; }

        public IEnumerable<ReviewServiceModel> Reviews { get; set; }
            = new List<ReviewServiceModel>();
    }
}
