using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using static MovieHub.Data.Common.DataConstants;

namespace MovieHub.Data.Models
{
    public class Review
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(ReviewContentMaxLength)]
        public string Content { get; set; } = string.Empty;

        [Range(RatingMin, RatingMax)]
        public int Rating { get; set; }

        public DateTime CreatedOn { get; set; }

        // ----- Relationship: the reviewed Movie -----
        public int MovieId { get; set; }

        [ForeignKey(nameof(MovieId))]
        public Movie Movie { get; set; } = null!;

        // ----- Relationship: the User who wrote the review -----
        [Required]
        public string AuthorId { get; set; } = string.Empty;

        [ForeignKey(nameof(AuthorId))]
        public ApplicationUser Author { get; set; } = null!;
    }
}
