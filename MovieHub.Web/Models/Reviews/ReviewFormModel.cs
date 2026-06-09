using System.ComponentModel.DataAnnotations;

using static MovieHub.Data.Common.DataConstants;

namespace MovieHub.Web.Models.Reviews
{
    public class ReviewFormModel
    {
        public int MovieId { get; set; }

        [Required]
        [StringLength(ReviewContentMaxLength, MinimumLength = ReviewContentMinLength)]
        public string Content { get; set; } = string.Empty;

        [Range(RatingMin, RatingMax)]
        public int Rating { get; set; }
    }
}
