using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lista12.Models
{
    public class Article
    {
        public int ID { get; set; }

        [StringLength(100, MinimumLength = 3)]
        [Required]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }

        [Display(Name = "Photo")] public string ImagePath { get; set; }

        [Display(Name = "Category")]
        public int? CategoryId { get; set; }

        public Category Category { get; set; }

        [NotMapped]
        public IFormFile Image { get; set; }
    }
}