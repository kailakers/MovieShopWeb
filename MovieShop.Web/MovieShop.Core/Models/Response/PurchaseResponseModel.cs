using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace MovieShop.Core.Models.Response
{
    public class PurchaseResponseModel
    {
        [Required]
        public int UserId { get; set; }
        
        [AllowNull]
        public Guid PurchaseNumber { get; set; }
        public decimal? TotalPrice { get; set; }
        public DateTime? PurchaseDateTime { get; set; }
        [Required]
        public int MovieId { get; set; }
    }
}