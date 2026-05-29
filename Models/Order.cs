using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    public class Order
    {   

        public enum OrderStatus
        {
            Pending,
            Processing,
            Shipped,
            Delivered,
            Cancelled
        }

        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Today;
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }
        [ValidateNever]
        public ApplicationUser User { get; set; } //Navigation property
        [ValidateNever]
        public string UserId { get; set; } //Foreign key
        public OrderStatus Status { get; set; } = OrderStatus.Pending;//default setovano
        public string? ShippingAddress { get; set; }
        public List<OrderItem> OrderItems { get; set; } = new();

    }
}
