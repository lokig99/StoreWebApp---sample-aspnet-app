using Lista12.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StoreWebApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreWebApp.Controllers
{
    public class ShopController : Controller
    {
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

            int selectedCategory = TempData["selectedCat"] as int? ?? -1;
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


    }
}
