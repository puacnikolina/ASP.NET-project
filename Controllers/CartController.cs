using Microsoft.AspNetCore.Mvc;
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


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout()
        {
            var cart = GetCart();
            if (!cart.Items.Any())
            {
                TempData["Error"] = "Your cart is empty.";
                return RedirectToAction("Index");
            }

            // Get logged in user ID
            var userId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                // Must be logged in to checkout
                return Redirect("/Identity/Account/Login");
            }

            var newOrder = new Order
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                TotalAmount = cart.TotalPrice,
                Status = Order.OrderStatus.Pending
            };

            _context.Orders.Add(newOrder);

            foreach (var cartItem in cart.Items)
            {
                var orderItem = new OrderItem
                {
                    Order = newOrder, 
                    ProductId = cartItem.ProductId,
                    Quantity = cartItem.Quantity,
                    UnitPrice = cartItem.Price
                };
                _context.OrderItems.Add(orderItem);
            }

            await _context.SaveChangesAsync();

            HttpContext.Session.Remove(CartSessionKey);
            TempData["Success"] = "Thank you for your purchase!";

            return RedirectToAction("Checkout");
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
