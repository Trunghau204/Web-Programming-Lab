using System;
using System.ComponentModel.DataAnnotations;

namespace Lab02_WebsiteBanHang.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required, StringLength(50)]
        public string Name { get; set; } = string.Empty;


    }
}
