namespace Project.Models.ViewModels
{
    public class CartViewModel
    {

        public List<CartItem> Items { get; set; } = new();
        public decimal TotalPrice => Items.Sum(i => i.TotalPrice);
        public int ItemCount => Items.Sum(i => i.Quantity);
        public bool IsEmpty => Items.Count == 0;



    }
}
