using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Project.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        [StringLength(500)]
        public string? Description { get; set; }
        public List<Product> Products { get; set; } = new();//property za navigaciju
    }
}
