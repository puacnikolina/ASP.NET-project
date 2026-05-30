using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Models;

namespace Project.Controllers
{
    public class ProductController(ApplicationDbContext context) : Controller
    {
        //Dependency Injection of the database context
        private readonly ApplicationDbContext _context = context;

        //method to display the list of products with optional filtering by category and search string
        public async Task<IActionResult> Index(int? categoryId, string? searchString)
        {
            var productQuery = _context.Products
                .Include(p => p.Category)
                .Where(p => p.IsActive);

            if (categoryId.HasValue)
            {
                productQuery = productQuery.Where(p => p.CategoryId == categoryId);
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                productQuery = productQuery.Where(p => p.Name.Contains(searchString) || (p.Description != null && p.Description.Contains(searchString)));
            }

            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.CurrentCategory = categoryId;
            ViewBag.SearchString = searchString;

            return View(await productQuery.ToListAsync());
        }

        //GET: Product/Details/5
        public async Task<IActionResult> Details(int? id)
        { 
            if(id == null) 
            {
                return NotFound();
            }
            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }
            
            return View(product);

        }



    }
}
