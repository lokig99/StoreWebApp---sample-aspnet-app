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

        [DataType(DataType.Currency)]
        [Required]
        public decimal Price { get; set; }

        [Display(Name = "Image path")] public string ImagePath { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}