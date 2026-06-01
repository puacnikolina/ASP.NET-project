using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using static System.Net.WebRequestMethods;

namespace Project.Models
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options) 
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Product> Products { get; set; }

        public DbSet<Category> Categories { get; set; }

        //model za hardkodovanje proizvoda u bazu moze posle da se skloni
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
                 new Product { ProductId = 2,Name = "Concordia",Description = "A strategic economic board game set in the Roman Empire where players expand their trade network, manage resources, and compete for influence across ancient provinces.",Price = 54.99m,StockQuantity = 45,CategoryId = 1,ImageUrl = "https://images.unsplash.com/photo-1585504198199-20277593b94f?q=80&w=1317&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D"},
                 new Product { ProductId = 3, Name = "Forbidden Island", Description = "Work together as a team of adventurers to recover ancient treasures and escape a sinking island before it's too late.", Price = 29.99m, StockQuantity = 75, CategoryId = 2, ImageUrl = "https://images.unsplash.com/photo-1653080583976-b25842d831c6?q=80&w=1170&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D" },
                 new Product { ProductId = 4, Name = "Codenames", Description = "A social word game where teams compete to identify secret agents using clever one-word clues and deduction.", Price = 19.99m, StockQuantity = 100, CategoryId = 2, ImageUrl = "https://images.unsplash.com/photo-1733297190314-b49a52ac1f98?q=80&w=688&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D" },
                 new Product { ProductId = 5, Name = "Coup", Description = "A fast-paced bluffing card game of deception, influence, and political intrigue where only the most cunning player survives.", Price = 14.99m, StockQuantity = 120, CategoryId = 3, ImageUrl = "https://images.unsplash.com/photo-1659480140108-5484eb035688?q=80&w=687&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D" }
            );
          
        }
    }
}
