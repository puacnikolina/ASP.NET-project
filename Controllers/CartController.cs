using Microsoft.AspNetCore.Mvc;
using Project.Extensions;
using Project.Models;

namespace Project.Controllers
{
    public class CartController(ApplicationDbContext context) : Controller
    {

        private readonly ApplicationDbContext _context = context;
        private const string CartSessionKey = "ShoppingCart";

        //helper metoda koja ucitava cart iz sessiona, ako nema cart u sessionu, vraca novi prazan cart
        private void SaveCart(Models.ViewModels.CartViewModel cart)
        {
            HttpContext.Session.SetObject(CartSessionKey, cart);
        }
    }
}
