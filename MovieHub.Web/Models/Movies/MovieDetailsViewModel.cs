using MovieHub.Services.Models.Movies;

namespace MovieHub.Web.Models.Movies
{
    public class MovieDetailsViewModel
    {
        public MovieDetailsServiceModel Movie { get; set; } = null!;

        public bool IsFavorite { get; set; }
    }
}
