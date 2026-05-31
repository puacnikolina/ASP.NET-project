using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    [NotMapped] // This class is not mapped to the database, it's used for the shopping cart in session
    public class CartItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string? ImageUrl { get; set; }
        public decimal TotalPrice => Price * Quantity;

    }
}
