using System.ComponentModel.DataAnnotations;

using static MovieHub.Data.Common.DataConstants;

namespace MovieHub.Data.Models
{
    public class Genre
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(GenreNameMaxLength)]
        public string Name { get; set; } = string.Empty;

        // One Genre has many Movies (one-to-many).
        public ICollection<Movie> Movies { get; set; } = new List<Movie>();
    }
}
