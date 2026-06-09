using System.ComponentModel.DataAnnotations;

using static MovieHub.Data.Common.DataConstants;

namespace MovieHub.Web.Models.Genres
{
    public class GenreFormModel
    {
        [Required]
        [StringLength(GenreNameMaxLength, MinimumLength = GenreNameMinLength)]
        public string Name { get; set; } = string.Empty;
    }
}
