using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatalogoAPI.Migrations
{
    /// <inheritdoc />
    public partial class PopulateProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Insert into Products(Name, Description, Price, ImageUrl, Stock, CreatedAt, CategoryId) Values('Coca-Cola', 'Cola soda drink 350ml', 5.45, 'coca-cola.jpg', 50, now(), 1)");

            migrationBuilder.Sql("Insert into Products(Name, Description, Price, ImageUrl, Stock, CreatedAt, CategoryId) Values('X-Burger', 'Sandwich with hamburger and cheese 300g', 9.99, 'x-burger.jpg', 50, now(), 2)");

            migrationBuilder.Sql("Insert into Products(Name, Description, Price, ImageUrl, Stock, CreatedAt, CategoryId) Values('Milk Pudding', 'Milk pudding 100g', 4.99, 'milk-pudding.jpg', 30, now(), 3)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Delete from Products");
        }
    }
}
