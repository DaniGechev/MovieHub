namespace MovieHub.Services.Models.Movies
{
    public class MovieListServiceModel
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string ImageUrl { get; set; } = string.Empty;

        public int ReleaseYear { get; set; }

        public string GenreName { get; set; } = string.Empty;
    }
}
