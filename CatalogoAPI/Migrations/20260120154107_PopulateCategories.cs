using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatalogoAPI.Migrations
{
    /// <inheritdoc />
    public partial class PopulateCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Insert into Categories(Name, ImageUrl) Values('Drinks', 'drinks.jpg')");
            migrationBuilder.Sql("Insert into Categories(Name, ImageUrl) Values('Snacks', 'snacks.jpg')");
            migrationBuilder.Sql("Insert into Categories(Name, ImageUrl) Values('Desserts', 'desserts.jpg')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Delete from Categories");
        }
    }
}
