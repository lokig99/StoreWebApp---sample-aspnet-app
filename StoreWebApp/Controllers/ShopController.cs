using Lista12.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StoreWebApp.Data;
using StoreWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreWebApp.Controllers
{
    public class ShopController : Controller
    {
        public const string CartItemsTag = "cti";


        private readonly StoreDbContext _context;
        [BindProperty(SupportsGet = true)]
        public Category SelectedCategory { get; set; }

        public ShopController(StoreDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "ID", "Name");

            int selectedCategory = TempData.Peek("selectedCat") as int? ?? -1;
            ViewData["currentCat"] = selectedCategory;

            if (selectedCategory != -1)
            {
                var storeDbContext = _context.Articles.Where(a => a.CategoryId == selectedCategory);
                return View(await storeDbContext.ToListAsync());
            }

            return View(new List<Article>());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(int category)
        {
            TempData["selectedCat"] = category;
            return RedirectToAction(nameof(Index));
        }


        public IActionResult AddCart(int articleID)
        {
            if (HttpContext.Session.GetString(CartItemsTag) == null)
            {
                var tmp = new Dictionary<int, int>();
                HttpContext.Session.SetString(CartItemsTag, JsonConvert.SerializeObject(tmp));
            }

            var items = JsonConvert.DeserializeObject<Dictionary<int, int>>(HttpContext.Session.GetString(CartItemsTag));
            items[articleID] = items.GetValueOrDefault(articleID, 0) + 1;
            HttpContext.Session.SetString(CartItemsTag, JsonConvert.SerializeObject(items));

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> ShoppingCart()
        {
            if (HttpContext.Session.GetString(CartItemsTag) == null)
            {
                return View(new List<CartItem>());
            }

            var itemIDs = JsonConvert.DeserializeObject<Dictionary<int, int>>(HttpContext.Session.GetString(CartItemsTag));
            var articles = _context.Articles.Join(itemIDs,
                                                 article => article.ID,
                                                 pair => pair.Key,
                                                 (article, pair) => new CartItem(article, pair.Key));

            return View(await articles.ToListAsync());
        }


    }


}
