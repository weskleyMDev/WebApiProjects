using CatalogoAPI.Context;
using CatalogoAPI.Filter;
using CatalogoAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CatalogoAPI.Controllers;

[Route("[controller]")]
[ApiController]
public class CategoriesController(AppDbContext context, IConfiguration configuration, ILogger<CategoriesController> logger) : ControllerBase
{
    private readonly AppDbContext _context = context;
    private readonly IConfiguration _configuration = configuration;
    private readonly ILogger<CategoriesController> _logger = logger;

    [HttpGet("config")]
    public ActionResult<string> GetConfigValue()
    {
        var value = _configuration["key1"];
        var subkey2 = _configuration["section1:subkey2"];
        return $"{value} - {subkey2}" ?? "Key not found";
    }

    [HttpGet]
    [ServiceFilter(typeof(ApiLoggingFilter))]
    public async Task<ActionResult<IEnumerable<Category>>> Get()
    {
        var categories = await _context.Categories.Take(10).ToListAsync();
        if (categories is null)
        {
            return NotFound();
        }
        return categories;
    }

    [HttpGet("{id:int:min(1)}", Name = "GetCategoryById")]
    public ActionResult<Category> Get(int id)
    {
        //throw new Exception("Test exception handling middleware");
        var category = _context.Categories.FirstOrDefault(c => c.CategoryId == id);
        if (category is null)
        {
            return NotFound($"Category {id} not found!");
        }
        return category;
    }

    [HttpGet("products")]
    public ActionResult<IEnumerable<Category>> GetCategoriesWithProducts()
    {
        _logger.LogInformation("##### Getting categories with products #####");
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

    [HttpPut("{id:int:min(1)}")]
    public ActionResult Put(int id, Category category)
    {
        if (id != category.CategoryId)
        {
            return BadRequest();
        }
        _context.Entry(category).State = EntityState.Modified;
        _context.SaveChanges();
        return Ok(category);
    }

    [HttpDelete("{id:int:min(1)}")]
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