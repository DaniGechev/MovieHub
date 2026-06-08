using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using static MovieHub.Data.Common.DataConstants;

namespace MovieHub.Data.Models
{
    public class Movie
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(MovieTitleMaxLength)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(MovieDescriptionMaxLength)]
        public string Description { get; set; } = string.Empty;

        public int ReleaseYear { get; set; }

        [Required]
        [MaxLength(MovieDirectorMaxLength)]
        public string Director { get; set; } = string.Empty;

        [Required]
        [MaxLength(MovieImageUrlMaxLength)]
        public string ImageUrl { get; set; } = string.Empty;

        // ----- Relationship: Genre (many Movies -> one Genre) -----
        public int GenreId { get; set; }

        [ForeignKey(nameof(GenreId))]
        public Genre Genre { get; set; } = null!;

        // ----- Relationship: Reviews (one Movie -> many Reviews) -----
        public ICollection<Review> Reviews { get; set; } = new List<Review>();

        // ----- Relationship: Favorites (many-to-many with Users via Favorite) -----
        public ICollection<Favorite> FavoritedBy { get; set; } = new List<Favorite>();
    }
}
