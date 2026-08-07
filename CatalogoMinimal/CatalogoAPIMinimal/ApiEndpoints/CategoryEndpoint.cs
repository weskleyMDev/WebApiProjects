using CatalogoAPIMinimal.Context;
using CatalogoAPIMinimal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CatalogoAPIMinimal.ApiEndpoints;

public static class CategoryEndpoint
{
    public static void MapCategoryEndpoints(this WebApplication app)
    {
        app.MapPost("/categories", async ([FromBody] Category category, [FromServices] AppDbContext dataBase) =>
{
    dataBase.Categories.Add(category);
    await dataBase.SaveChangesAsync();
    return Results.Created($"/categories/{category.CategoryId}", category);
}).Accepts<Category>("application/json")
  .Produces<Category>(StatusCodes.Status201Created)
  .WithName("CreateCategory")
  .WithTags("Categories");

        app.MapGet("/categories", async (AppDbContext dataBase) =>
        {
            var categories = await dataBase.Categories.AsNoTracking().ToListAsync();
            return Results.Ok(categories);
        }).WithTags("Categories")
          .RequireAuthorization();

        app.MapGet("/categories/{id:int}", async (int id, AppDbContext dataBase) =>
        {
            var category = await dataBase.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.CategoryId == id);
            return category is not null ? Results.Ok(category) : Results.NotFound($"Category with ID = {id} not found!");
        }).WithTags("Categories");

        app.MapPut("/categories/{id:int}", async (int id, Category category, AppDbContext dataBase) =>
        {
            if (category.CategoryId != id)
            {
                return Results.BadRequest("Category ID mismatch!");
            }

            var updatedCategory = await dataBase.Categories.FindAsync(id);

            if (updatedCategory is null)
            {
                return Results.NotFound($"Category with ID = {id} not found!");
            }

            updatedCategory.Name = category.Name;
            updatedCategory.Description = category.Description;

            await dataBase.SaveChangesAsync();
            return Results.Ok(updatedCategory);
        }).WithTags("Categories");

        app.MapDelete("/categories/{id:int}", async (int id, AppDbContext dataBase) =>
        {
            var category = await dataBase.Categories.FindAsync(id);

            if (category is null)
            {
                return Results.NotFound($"Category with ID = {id} not found!");
            }

            dataBase.Categories.Remove(category);
            await dataBase.SaveChangesAsync();
            return Results.NoContent();
        }).WithTags("Categories");
    }
}