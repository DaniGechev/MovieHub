namespace MovieHub.Services.Models.Reviews
{
    public class ReviewServiceModel
    {
        public int Id { get; set; }

        public string Content { get; set; } = string.Empty;

        public int Rating { get; set; }

        public DateTime CreatedOn { get; set; }

        public string AuthorName { get; set; } = string.Empty;

        public int MovieId { get; set; }
    }
}
