using System;
using System.ComponentModel.DataAnnotations;
using Lab02_WebsiteBanHang.Models;

namespace Lab02_WebsiteBanHang.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required, StringLength(100)]
        public string Name { get; set; } = string.Empty;
        [Range(0.01, 10000.00)]
        public decimal Price { get; set; }
        public string Description { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public List<string>? ImageUrls { get; set; }
        // public List<ProductImage>? Images { get; set; }
        public int CategoryId { get; set; }
        // public Category? Category { get; set; }
    }
}
