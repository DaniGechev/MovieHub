namespace MovieHub.Services.Models.Movies
{
    /// <summary>
    /// Plain data carrier used when creating or editing a movie. The user-facing
    /// validation attributes live on the Web layer's input model; this DTO just
    /// moves the values into the service.
    /// </summary>
    public class MovieFormServiceModel
    {
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int ReleaseYear { get; set; }

        public string Director { get; set; } = string.Empty;

        public string ImageUrl { get; set; } = string.Empty;

        public int GenreId { get; set; }
    }
}
