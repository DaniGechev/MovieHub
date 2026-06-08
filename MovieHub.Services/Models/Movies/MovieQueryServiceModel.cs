namespace MovieHub.Services.Models.Movies
{
    /// <summary>
    /// Carries one page of movies plus the total count, so the controller can
    /// build pagination (this powers the optional searching/sorting/paging bonus).
    /// </summary>
    public class MovieQueryServiceModel
    {
        public int TotalMoviesCount { get; set; }

        public IEnumerable<MovieListServiceModel> Movies { get; set; }
            = new List<MovieListServiceModel>();
    }
}
