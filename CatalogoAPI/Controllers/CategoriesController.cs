using CatalogoAPI.Context;
using CatalogoAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CatalogoAPI.Controllers;

[Route("[controller]")]
[ApiController]
public class CategoriesController(AppDbContext context) : ControllerBase
{
    private readonly AppDbContext _context = context;

    [HttpGet]
    public ActionResult<IEnumerable<Category>> Get()
    {
        var categories = _context.Categories.Take(10).ToList();
        if (categories is null)
        {
            return NotFound();
        }
        return categories;
    }

    [HttpGet("{id:int}", Name = "GetCategoryById")]
    public ActionResult<Category> Get(int id)
    {
        var category = _context.Categories.FirstOrDefault(c => c.CategoryId == id);
        if (category is null)
        {
            return NotFound("Category not found!");
        }
        return category;
    }

    [HttpGet("products")]
    public ActionResult<IEnumerable<Category>> GetCategoriesWithProducts()
    {
        var categories = _context.Categories.Include(c => c.Products).Where(c => c.CategoryId <= 5).ToList();
        if (categories is null)
        {
            return NotFound();
        }
        return categories;
    }

    [HttpPost]
    public ActionResult Post(Category category)
    {
        if (category is null)
        {
            return BadRequest();
        }
        _context.Categories.Add(category);
        _context.SaveChanges();
        return new CreatedAtRouteResult("GetCategoryById", new { id = category.CategoryId }, category);
    }

    [HttpPut("{id:int}")]
    public ActionResult Put(int id, Category category)
    {
        if (id != category.CategoryId)
        {
            return BadRequest();
        }
        _context.Entry(category).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        _context.SaveChanges();
        return Ok(category);
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        var category = _context.Categories.FirstOrDefault(c => c.CategoryId == id);
        if (category is null)
        {
            return NotFound("Category not found!");
        }
        _context.Categories.Remove(category);
        _context.SaveChanges();
        return Ok(category);
    }

}