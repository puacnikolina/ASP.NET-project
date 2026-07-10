using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Project.Models;
using Project.Models.ViewModels;

namespace Project.Controllers
{
    //samo ulogovani korisnici sa Admin rolom mogu da pristupe ovom kontroleru
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Admin/CreateProduct - shows a form for creating a new product
        public async Task<IActionResult> CreateProduct()
        {
            ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "CategoryId", "Name");
            return View();
        }

        // POST: Admin/CreateProduct - handles form submission and image upload
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProduct(Product product, IFormFile? imageFile)
        {
            ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "CategoryId", "Name");

            if (!ModelState.IsValid)
            {
                return View(product);
            }

            // If user provided an external image URL, use it. Otherwise, if a file was uploaded, save it to wwwroot.
            if (string.IsNullOrWhiteSpace(product.ImageUrl))
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "products");
                    Directory.CreateDirectory(uploads);
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                    var filePath = Path.Combine(uploads, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    product.ImageUrl = "/images/products/" + fileName;
                }
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Product created.";
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin
        public async Task<IActionResult> Index()
        {
            var model = new AdminDashboardViewModel
            {
                TotalUsers = await _userManager.Users.CountAsync(),
                BannedUsers = await _userManager.Users.CountAsync(u => u.LockoutEnd != null && u.LockoutEnd > DateTimeOffset.UtcNow),
                TotalOrders = await _context.Orders.CountAsync(),
                PendingOrders = await _context.Orders.CountAsync(o => o.Status == Order.OrderStatus.Pending),
                ProcessingOrders = await _context.Orders.CountAsync(o => o.Status == Order.OrderStatus.Processing),
                TotalRevenue = await _context.Orders
                    .Where(o => o.Status != Order.OrderStatus.Cancelled)
                    .SumAsync(o => o.TotalAmount)
            };

            return View(model);
        }

        // GET: Admin/Stats - ko kupuje sta, po proizvodu
        public async Task<IActionResult> Stats()
        {
            var stats = await _context.OrderItems
                .Include(oi => oi.Order)
                .Include(oi => oi.Product)
                .Where(oi => oi.Order.Status != Order.OrderStatus.Cancelled)
                .GroupBy(oi => new { oi.ProductId, oi.Product.Name })
                .Select(g => new ProductStatViewModel
                {
                    ProductId = g.Key.ProductId,
                    ProductName = g.Key.Name,
                    TotalQuantitySold = g.Sum(oi => oi.Quantity),
                    TotalRevenue = g.Sum(oi => oi.Quantity * oi.UnitPrice),
                    OrderCount = g.Select(oi => oi.OrderId).Distinct().Count()
                })
                .OrderByDescending(s => s.TotalQuantitySold)
                .ToListAsync();

            return View(stats);
        }

        // GET: Admin/Orders - svi orderi, ne samo trenutnog korisnika
        public async Task<IActionResult> Orders()
        {
            var orders = await _context.Orders
                .Include(o => o.User)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            return View(orders);
        }

        // POST: Admin/UpdateOrderStatus
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, Order.OrderStatus status)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
            {
                return NotFound();
            }

            order.Status = status;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Orders));
        }

        // GET: Admin/Users
        public IActionResult Users()
        {
            var users = _userManager.Users.OrderBy(u => u.Email).ToList();
            return View(users);
        }

        // POST: Admin/ToggleBan
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleBan(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            // ne dozvoljavamo da admin banuje sebe ili drugog admina
            var currentUserId = _userManager.GetUserId(User);
            if (user.Id == currentUserId || await _userManager.IsInRoleAsync(user, "Admin"))
            {
                TempData["Error"] = "You cannot ban an admin account.";
                return RedirectToAction(nameof(Users));
            }

            // Use built-in lockout to ban/unban users
            if (user.LockoutEnd.HasValue && user.LockoutEnd > DateTimeOffset.UtcNow)
            {
                // currently banned -> unban
                await _userManager.SetLockoutEndDateAsync(user, null);
            }
            else
            {
                // ban indefinitely
                await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
                await _userManager.SetLockoutEnabledAsync(user, true);
            }

            await _userManager.UpdateAsync(user);

            return RedirectToAction(nameof(Users));
        }


    }
}
