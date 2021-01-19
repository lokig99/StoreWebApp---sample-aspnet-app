using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lista12.Models
{
    public class Category
    {
        public int ID { get; set; }
        [StringLength(100)] public string Name { get; set; }
        public IList<Article> Articles { get; set; }
    }
}