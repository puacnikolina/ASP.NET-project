using Microsoft.AspNetCore.Identity;

namespace Project.Models
{
    public class ApplicationUser : IdentityUser
    {
        public List<Order> Orders { get; set; } = new(); //creates one-many relationship between ApplicationUser and Order
    }
}
