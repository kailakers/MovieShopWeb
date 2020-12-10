using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MovieShop.Core.Entities;

namespace MovieShop.Core.Models
{
    public class MovieCreateRequest
    {
        [StringLength(50)]
        [Required]
        public string Title { get; set; }
        
        [StringLength(2084)]
        public string Overview { get; set; }
        
        [Url]
        [StringLength(150)]
        public string BackdropUrl { get; set; }
        
        [Range(0, 500000000)]
        public decimal? Budget { get; set; }
        
        public DateTime? CreatedDate { get; set; }
        
        [Url]
        public string ImdbUrl { get; set; }
        
        [StringLength(20)]
        public string OriginalLanguage { get; set; }
        
        [Required]
        [Url]
        public string PosterUrl { get; set; }
        
        public decimal? Price { get; set; }
        
        public DateTime? ReleaseDate { get; set; }
        
        public int? RunTime { get; set; }
        
        [StringLength(2084)]
        public string TagLine { get; set; }
        
        [Url]
        public string TmdbUrl { get; set; }
    }
}