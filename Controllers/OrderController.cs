
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Models;
using System.Security.Claims;

//treba biti ulogovan da moze da se pristupi kontroleru
[Authorize]
public class OrderController : Controller
{
    private readonly ApplicationDbContext _context;

    public OrderController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: ORDERS
    public async Task<IActionResult> Index()
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return View(await _context.Orders
            .Include(o => o.User)
            .Where(o => o.UserId == currentUserId)
            .ToListAsync()); 
    }

    // GET: ORDERS/Details/5
    public async Task<IActionResult> Details(int? orderid)
    {
        if (orderid == null)
        {
            return NotFound();
        }

        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // da moze da se display - uzima sve iteme
        var order = await _context.Orders
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
            .FirstOrDefaultAsync(m => m.OrderId == orderid && m.UserId == currentUserId);

        if (order == null)
        {
            return NotFound();
        }

        return View(order);
    }

    // GET: ORDERS/Create
    public IActionResult Create()
    {
        return View();
    }

    
    //mozda dodati metodu za promenu statusa porudzbine i staviti da to moze admin da radi 




   //// GET: ORDERS/Delete/5
   // public async Task<IActionResult> Delete(int? orderid)
   // {
   //     if (orderid == null)
   //     {
   //         return NotFound();
   //     }

   //     var order = await _context.Orders
   //         .FirstOrDefaultAsync(m => m.OrderId == orderid);
   //     if (order == null)
   //     {
   //         return NotFound();
   //     }

   //     return View(order);
   // }

   // // POST: ORDERS/Delete/5
   // [HttpPost, ActionName("Delete")]
   // [ValidateAntiForgeryToken]
   // public async Task<IActionResult> DeleteConfirmed(int? orderid)
   // {
   //     var order = await _context.Orders.FindAsync(orderid);
   //     if (order != null)
   //     {
   //         _context.Orders.Remove(order);
   //     }

   //     await _context.SaveChangesAsync();
   //     return RedirectToAction(nameof(Index));
   // }

   // private bool OrderExists(int? orderid)
   // {
   //     return _context.Orders.Any(e => e.OrderId == orderid);
   // }
}
