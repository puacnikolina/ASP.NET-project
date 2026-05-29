using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Project.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Today;
        public decimal TotalAmount { get; set; }
        [ValidateNever]
        public ApplicationUser User { get; set; } //Navigation property
        [ValidateNever]
        public string UserId { get; set; } //Foreign key

    }
}
