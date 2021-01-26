using System;
using Lista12.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StoreWebApp.Data;
using StoreWebApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreWebApp.Controllers
{
    public class ShopController : Controller
    {
        private const string CartItemsTag = "cart";
        private readonly CookieOptions _options = new CookieOptions();
        private readonly StoreDbContext _context;

        public ShopController(StoreDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewData["AddedItem"] = TempData["AddedItem"];
            ViewData["ShowAlert"] = TempData["ShowAlert"];
            ViewData["CategoryId"] = new SelectList(_context.Categories, "ID", "Name");

            var selectedCategory = TempData.Peek("selectedCat") as int? ?? -1;
            ViewData["currentCat"] = selectedCategory;

            if (selectedCategory == -1) return View(new List<Article>());
            var storeDbContext = _context.Articles.Where(a => a.CategoryId == selectedCategory);
            return View(await storeDbContext.ToListAsync());
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
            if (!string.IsNullOrEmpty(Request.Cookies[CartItemsTag]))
            {
                var items =
                    JsonConvert.DeserializeObject<Dictionary<int, int>>(Request.Cookies[CartItemsTag]);
                items[articleID] = items.GetValueOrDefault(articleID, 0) + 1;
                _options.Expires = DateTime.Now.AddDays(7);
                Response.Cookies.Append(CartItemsTag, JsonConvert.SerializeObject(items), _options);
            }
            else
            {
                var tmp = new Dictionary<int, int> {[articleID] = 1};
                _options.Expires = DateTime.Now.AddDays(7);
                Response.Cookies.Append(CartItemsTag, JsonConvert.SerializeObject(tmp), _options);
            }

            TempData["AddedItem"] = _context.Articles.First(a => a.ID == articleID).Name;
            TempData["ShowAlert"] = "show";

            return RedirectToAction(nameof(Index));
        }

        public IActionResult DeleteItem(int articleID, string articleName)
        {
            var items =
                JsonConvert.DeserializeObject<Dictionary<int, int>>(Request.Cookies[CartItemsTag]);

            TempData["RemovedItem"] = articleName;
            TempData["RemovedCount"] = items[articleID];
            TempData["ShowAlert"] = "show";

            items.Remove(articleID);
            _options.Expires = DateTime.Now.AddDays(7);
            Response.Cookies.Append(CartItemsTag, JsonConvert.SerializeObject(items), _options);
            return RedirectToAction(nameof(ShoppingCart));
        }

        public IActionResult ItemCountUpdate(int articleID, string articleName, bool wasAdded)
        {
            var items =
                JsonConvert.DeserializeObject<Dictionary<int, int>>(Request.Cookies[CartItemsTag]);

            if (!wasAdded && items[articleID] == 1)
            {
                return RedirectToAction(nameof(DeleteItem), new {articleID, articleName});
            }

            items[articleID] += wasAdded ? 1 : -1;
            _options.Expires = DateTime.Now.AddDays(7);
            Response.Cookies.Append(CartItemsTag, JsonConvert.SerializeObject(items), _options);
            return RedirectToAction(nameof(ShoppingCart));
        }

        [HttpGet]
        public async Task<IActionResult> ShoppingCart()
        {
            if (Request.Cookies[CartItemsTag] == null)
            {
                return View(new List<CartItem>());
            }

            ViewBag.RemovedItem = TempData["RemovedItem"];
            ViewBag.RemovedCount = TempData["RemovedCount"];
            ViewBag.ShowAlert = TempData["ShowAlert"];

            var items =
                JsonConvert.DeserializeObject<Dictionary<int, int>>(Request.Cookies[CartItemsTag]);

            var itemIDs = items.Select(pair => pair.Key).ToHashSet();

            var articles = await _context.Articles.Where(a => itemIDs.Contains(a.ID)).Include(a => a.Category)
                .ToListAsync();

            var total = articles.Select(a => a.Price * items[a.ID]).Sum();
            ViewBag.Total = $"{total:C}";

            var cartItems = articles.Select(a => new CartItem(a, items[a.ID]));

            return View(cartItems.ToList());
        }
    }
}