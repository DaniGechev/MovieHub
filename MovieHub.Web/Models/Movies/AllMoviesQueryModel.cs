using System.ComponentModel.DataAnnotations;

using MovieHub.Services.Models.Movies;

namespace MovieHub.Web.Models.Movies
{
    public class AllMoviesQueryModel
    {
        public const int MoviesPerPage = 12;

        [Display(Name = "Search")]
        public string? SearchTerm { get; set; }

        public MovieSorting Sorting { get; set; }

        public int CurrentPage { get; set; } = 1;

        public int TotalMoviesCount { get; set; }

        public int TotalPages
            => (int)Math.Ceiling((double)this.TotalMoviesCount / MoviesPerPage);

        public IEnumerable<MovieListServiceModel> Movies { get; set; }
            = new List<MovieListServiceModel>();
    }
}