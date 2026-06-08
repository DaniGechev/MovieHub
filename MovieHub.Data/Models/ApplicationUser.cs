using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Identity;

using static MovieHub.Data.Common.DataConstants;

namespace MovieHub.Data.Models
{
    /// <summary>
    /// Extends the built-in ASP.NET Identity user with project-specific
    /// (custom) properties. This satisfies the "add custom properties for
    /// the user" requirement.
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [MaxLength(NickNameMaxLength)]
        public string NickName { get; set; } = string.Empty;

        public string? ProfilePictureUrl { get; set; }

        // Reviews written by this user.
        public ICollection<Review> Reviews { get; set; } = new List<Review>();

        // Movies this user has marked as favorite (the many-to-many side).
        public ICollection<Favorite> FavoriteMovies { get; set; } = new List<Favorite>();
    }
}
