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
  .WithTags("Setter");

app.MapGet("/categories", async (AppDbContext dataBase) =>
{
    var categories = await dataBase.Categories.AsNoTracking().ToListAsync();
    return Results.Ok(categories);
});

app.MapGet("/categories/{id:int}", async (int id, AppDbContext dataBase) =>
{
    var category = await dataBase.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.CategoryId == id);
    return category is not null ? Results.Ok(category) : Results.NotFound($"Category with ID = {id} not found!");
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();