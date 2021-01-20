using Lista12.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreWebApp.Models
{
    public class CartItem
    {
        public Article Article { get; set; }
        public int Count { get; set; }

        public CartItem(Article article, int count)
        {
            Article = article;
            Count = count;
        }
    }
}
