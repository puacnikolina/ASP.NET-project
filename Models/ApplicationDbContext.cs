using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Project.Models
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options) 
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Product> Products { get; set; }

        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Category>().HasData(
                new Category { CategoryId = 1, Name = "Strategy Games", Description = "Complex games that require strategic thinking and planning." },
                new Category { CategoryId = 2, Name = "Family Games", Description = "Fun games suitable for all ages and family gatherings." },
                new Category { CategoryId = 3, Name = "Card Games", Description = "Games played primarily with cards." }
            );

            builder.Entity<Product>().HasData(
                 new Product { ProductId = 1, Name = "Catan", Description = "Build settlements, trade resources, and expand your civilization on the island of Catan.", Price = 44.99m, StockQuantity = 60, CategoryId = 1, ImageUrl = "https://images.unsplash.com/photo-1667118398882-fe8fd62665c6?q=80&w=1331&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D" },
                 new Product { ProductId = 2, Name = "Carcassonne", Description = "Tile-placement game where players build the medieval landscape of Carcassonne.", Price = 34.99m, StockQuantity = 80, CategoryId = 2,ImageUrl= "https://images.unsplash.com/photo-1635921479440-f7a2c10d2d54?q=80&w=1331&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D" },
                 new Product { ProductId = 3, Name = "Exploding Kittens", Description = "A strategic card game of chance where players try to avoid exploding kittens.", Price = 24.99m, StockQuantity = 120, CategoryId = 3,ImageUrl= "https://images.unsplash.com/photo-1616406919718-b7adc7d81af6?q=80&w=1074&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D" }

            );
          
        }
    }
}
