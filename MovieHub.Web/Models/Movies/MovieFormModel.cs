using System.ComponentModel.DataAnnotations;

using static MovieHub.Data.Common.DataConstants;

namespace MovieHub.Web.Models.Movies
{
    public class MovieFormModel
    {
        [Required]
        [StringLength(MovieTitleMaxLength, MinimumLength = MovieTitleMinLength)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(MovieDescriptionMaxLength, MinimumLength = MovieDescriptionMinLength)]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "Release Year")]
        [Range(MovieMinReleaseYear, MovieMaxReleaseYear)]
        public int ReleaseYear { get; set; }

        [Required]
        [StringLength(MovieDirectorMaxLength, MinimumLength = MovieDirectorMinLength)]
        public string Director { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Image URL")]
        [StringLength(MovieImageUrlMaxLength)]
        [Url]
        public string ImageUrl { get; set; } = string.Empty;

        [Display(Name = "Genre")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a genre.")]
        public int GenreId { get; set; }

        // Populated by the controller to fill the dropdown; not posted back.
        public IEnumerable<MovieGenreViewModel> Genres { get; set; }
            = new List<MovieGenreViewModel>();
    }
}
