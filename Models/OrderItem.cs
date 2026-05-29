using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        //foregin keys
        public int OrderId { get; set; } 
        public int ProductId { get; set; }

        //navigation properties
        [ValidateNever]
        public Order Order { get; set; } = null!;
        [ValidateNever]
        public Product Product { get; set; } = null!;

        //computed property
        [NotMapped]
        public decimal TotalPrice => Quantity * UnitPrice;


    }
}
