namespace MovieShop.Core.Entities
{
    public class Trailer
    {
        public int Id { get; set; }
        //Foreign key from movie
        public int MovieId { get; set; }
        public string TrailerUrl { get; set; }
        public string Name { get; set; }
        
        //Navigation Properties: navigating to related entities
        //ex: trailer 24 => get me Movie Title, Movie Overview
        public Movie Movie { get; set; }
    }
}