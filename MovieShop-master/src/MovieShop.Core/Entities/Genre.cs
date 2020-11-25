using System.Collections.Generic;
using System.Text.Json.Serialization;
namespace MovieShop.Core.Entities
{
    public class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [JsonIgnore]
        public ICollection<MovieGenre> MovieGenres { get; set; }
    }
}