using CatalogoAPIMinimal.Context;
using CatalogoAPIMinimal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString)
    )
);

var app = builder.Build();

app.MapGet("/", () => "Starting this Minimal API!");

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
}).WithTags("Categories");

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
}).WithTags("Products");

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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();