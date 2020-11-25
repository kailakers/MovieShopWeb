namespace MovieShop.Core.Models
{
    public class ReviewMovieResponseModel
    {
        public int UserId { get; set; }
        public int MovieId { get; set; }
        public string ReviewText { get; set; }
        public decimal? Rating { get; set; }
        public string Name { get; set; }
    }
}