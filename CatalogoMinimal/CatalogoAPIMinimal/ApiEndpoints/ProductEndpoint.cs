using CatalogoAPIMinimal.Context;
using CatalogoAPIMinimal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CatalogoAPIMinimal.ApiEndpoints;

public static class ProductEndpoint
{
    public static void MapProductEndpoints(this WebApplication app)
    {
        app.MapPost("/products", async ([FromBody] Product product, [FromServices] AppDbContext dataBase) =>
{
    var categoryExists = await dataBase.Categories.AsNoTracking().AnyAsync(c => c.CategoryId == product.CategoryId);

    if (!categoryExists)
    {
        return Results.NotFound(new
        {
            message = $"Category ID = {product.CategoryId} not found!"
        });
    }

    dataBase.Products.Add(product);
    await dataBase.SaveChangesAsync();
    return Results.CreatedAtRoute("GetProductById", new
    {
        id = product.ProductId
    }, product);
}).Accepts<Product>("application/json")
  .Produces<Product>(StatusCodes.Status201Created)
  .Produces(StatusCodes.Status404NotFound)
  .WithName("CreateProduct")
  .WithTags("Products");

        app.MapGet("/products", async (AppDbContext dataBase) =>
        {
            var products = await dataBase.Products.AsNoTracking().ToListAsync();
            return Results.Ok(products);
        }).WithTags("Products")
          .RequireAuthorization();

        app.MapGet("/products/{id:int}", async (int id, AppDbContext dataBase) =>
        {
            var product = await dataBase.Products.AsNoTracking().FirstOrDefaultAsync(p => p.ProductId == id);
            return product is not null ? Results.Ok(product) : Results.NotFound(new
            {
                message = $"Product with ID = {id} not found!"
            });
        }).Produces<Product>(StatusCodes.Status200OK)
          .Produces(StatusCodes.Status404NotFound)
          .WithName("GetProductById")
          .WithTags("Products");

        app.MapPut("/products/{id:int}", async (int id, Product product, AppDbContext dataBase) =>
        {
            if (product.ProductId != id)
            {
                return Results.BadRequest("Product ID mismatch!");
            }

            var updatedProduct = await dataBase.Products.FindAsync(id);

            if (updatedProduct is null)
            {
                return Results.NotFound($"Product with ID = {id} not found!");
            }

            updatedProduct.Name = product.Name;
            updatedProduct.Description = product.Description;
            updatedProduct.Price = product.Price;
            updatedProduct.ImageUrl = product.ImageUrl;
            updatedProduct.Stock = product.Stock;
            updatedProduct.CreatedAt = DateTime.UtcNow;
            updatedProduct.CategoryId = product.CategoryId;

            await dataBase.SaveChangesAsync();
            return Results.Ok(updatedProduct);
        }).WithTags("Products");

        app.MapDelete("/products/{id:int}", async (int id, AppDbContext dataBase) =>
        {
            var product = await dataBase.Products.FindAsync(id);

            if (product is null)
            {
                return Results.NotFound($"Product with ID = {id} not found!");
            }

            dataBase.Products.Remove(product);
            await dataBase.SaveChangesAsync();
            return Results.NoContent();
        }).WithTags("Products");
    }
}