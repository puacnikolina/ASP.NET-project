using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public decimal Price { get; set; }

        [StringLength(200)]
        public string? ImageUrl { get; set; }

        [Required]
        [Range(0,int.MaxValue)]
        public int StockQuantity { get; set; }

        public bool IsActive { get; set; } = true; //default setovano

        [ValidateNever]
        public Category Category { get; set; } = null;

        public int CategoryId { get; set; } //Foreign key

        [ValidateNever]
        public List<OrderItem> OrderItems { get; set; } = new();


    }
}
