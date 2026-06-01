using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Project.Migrations
{
    /// <inheritdoc />
    public partial class products : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 2,
                columns: new[] { "CategoryId", "Description", "ImageUrl", "Name", "Price", "StockQuantity" },
                values: new object[] { 1, "A strategic economic board game set in the Roman Empire where players expand their trade network, manage resources, and compete for influence across ancient provinces.", "https://images.unsplash.com/photo-1585504198199-20277593b94f?q=80&w=1317&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", "Concordia", 54.99m, 45 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 3,
                columns: new[] { "CategoryId", "Description", "ImageUrl", "Name", "Price", "StockQuantity" },
                values: new object[] { 2, "Work together as a team of adventurers to recover ancient treasures and escape a sinking island before it's too late.", "https://images.unsplash.com/photo-1653080583976-b25842d831c6?q=80&w=1170&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", "Forbidden Island", 29.99m, 75 });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductId", "CategoryId", "Description", "ImageUrl", "IsActive", "Name", "Price", "StockQuantity" },
                values: new object[,]
                {
                    { 4, 2, "A social word game where teams compete to identify secret agents using clever one-word clues and deduction.", "https://images.unsplash.com/photo-1733297190314-b49a52ac1f98?q=80&w=688&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", true, "Codenames", 19.99m, 100 },
                    { 5, 3, "A fast-paced bluffing card game of deception, influence, and political intrigue where only the most cunning player survives.", "https://images.unsplash.com/photo-1659480140108-5484eb035688?q=80&w=687&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", true, "Coup", 14.99m, 120 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 5);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 2,
                columns: new[] { "CategoryId", "Description", "ImageUrl", "Name", "Price", "StockQuantity" },
                values: new object[] { 2, "Tile-placement game where players build the medieval landscape of Carcassonne.", "https://images.unsplash.com/photo-1635921479440-f7a2c10d2d54?q=80&w=1331&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", "Carcassonne", 34.99m, 80 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 3,
                columns: new[] { "CategoryId", "Description", "ImageUrl", "Name", "Price", "StockQuantity" },
                values: new object[] { 3, "A strategic card game of chance where players try to avoid exploding kittens.", "https://images.unsplash.com/photo-1616406919718-b7adc7d81af6?q=80&w=1074&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", "Exploding Kittens", 24.99m, 120 });
        }
    }
}
