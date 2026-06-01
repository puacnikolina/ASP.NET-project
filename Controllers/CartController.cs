using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Extensions;
using Project.Models;
using Project.Models.ViewModels;
using System.Security.Claims;

namespace Project.Controllers
{
    public class CartController(ApplicationDbContext context) : Controller
    {

        private readonly ApplicationDbContext _context = context;
        private const string CartSessionKey = "ShoppingCart";

        public IActionResult Index()
        {
            var cart = GetCart();
            return View(cart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null || !product.IsActive)
            {
                return NotFound();
            }

            var cart = GetCart();
            var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                cart.Items.Add(new CartItem
                {
                    ProductId = product.ProductId,
                    ProductName = product.Name,
                    Price = product.Price,
                    Quantity = quantity,
                    ImageUrl = product.ImageUrl
                });
            }
            product.StockQuantity -= quantity;
            await _context.SaveChangesAsync();
            SaveCart(cart);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateQuantity(int productId, int quantity)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null || !product.IsActive)
            {
                return NotFound();
            }

            var cart = GetCart();
            var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);

            if (item != null)
            {
                if (quantity > 0)
                {
                    int oldQuantity = item.Quantity;
                    int difference = quantity - oldQuantity;

                    // provera da li ima dovoljno na stanju
                    if (difference > 0 && product.StockQuantity < difference)
                    {
                        TempData["Error"] = "Not enough stock available.";
                        return RedirectToAction("Index");
                    }

                    item.Quantity = quantity;
                    product.StockQuantity -= difference;

                    await _context.SaveChangesAsync();
                }
                else
                {
                    // vracamo proizvode na stanje kada se izbrisu iz korpe
                    product.StockQuantity += item.Quantity;

                    cart.Items.Remove(item);

                    await _context.SaveChangesAsync();
                }

                SaveCart(cart);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveFromCart(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null || !product.IsActive)
            {
                return NotFound();
            }

            var cart = GetCart();
            var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);

            if (item != null)
            {
                cart.Items.Remove(item);
                product.StockQuantity += item.Quantity;
                await _context.SaveChangesAsync();
                SaveCart(cart);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ClearCart()
        {
            var cart = GetCart();

            foreach (var item in cart.Items)
            {
                var product = await _context.Products.FindAsync(item.ProductId);

                if (product != null)
                {
                    product.StockQuantity += item.Quantity;
                }
            }

            await _context.SaveChangesAsync();

            HttpContext.Session.Remove(CartSessionKey);

            return RedirectToAction("Index");
        }

        [Authorize]
        public async Task<IActionResult> Checkout()
        {
            var cart = GetCart();
            if(cart.IsEmpty) { return RedirectToAction("Index"); }
            return View(cart);
        }

        [HttpPost]
        [Authorize] //znaci da mora biti ulogovan da bi mogao da poziva ovo
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PlaceOrder(string? shippingAddress)
        {
            var cart = GetCart();
            if (cart.IsEmpty)
            {
                return RedirectToAction("Index");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var order = new Order
            {
                UserId = userId,
                TotalAmount = cart.TotalPrice,
                ShippingAddress = shippingAddress,
                Status = Order.OrderStatus.Pending,
                OrderDate = DateTime.Now

            };

            //dodavanje itema
            foreach(var item in cart.Items)
            {
                order.OrderItems.Add(new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.Price

                });

                var product = await _context.Products.FindAsync(item.ProductId);
                if (product != null)
                {
                    product.StockQuantity -= item.Quantity;
                }
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();


            //clearuje cart
            HttpContext.Session.Remove(CartSessionKey);
            return RedirectToAction("OrderConfirmation", new { orderId = order.OrderId });
        }

        [Authorize]
        public async Task<IActionResult> OrderConfirmation(int orderId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.OrderId == orderId && o.UserId == userId);

            if (order == null) { 
                return NotFound();
            }

            return View(order);

        }


        //helper metoda koja ucitava cart iz sessiona, ako nema cart u sessionu, vraca novi prazan cart
        private void SaveCart(CartViewModel cart)
        {
            HttpContext.Session.SetObject(CartSessionKey, cart);
        }

        private CartViewModel GetCart()
        {
            return HttpContext.Session.GetObject<CartViewModel>(CartSessionKey) ?? new CartViewModel();
        }
    }
}
