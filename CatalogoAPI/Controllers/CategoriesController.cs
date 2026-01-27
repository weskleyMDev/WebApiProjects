using CatalogoAPI.Context;
using CatalogoAPI.Filter;
using CatalogoAPI.Models;
using CatalogoAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CatalogoAPI.Controllers;

[Route("[controller]")]
[ApiController]
public class CategoriesController(ICategoryRepository repository, IConfiguration configuration, ILogger<CategoriesController> logger) : ControllerBase
{
    private readonly ICategoryRepository _repository = repository;
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
    public ActionResult<IEnumerable<Category>> Get()
    {
        var categories = _repository.GetCategories();
        if (categories is null)
        {
            return NotFound("No categories found!");
        }
        return Ok(categories);
    }

    [HttpGet("{id:int:min(1)}", Name = "GetCategoryById")]
    public ActionResult<Category> Get(int id)
    {
        var category = _repository.GetCategoryById(id);
        if (category is null)
        {
            return NotFound($"Category {id} not found!");
        }
        return Ok(category);
    }

    /* [HttpGet("products")]
    public ActionResult<IEnumerable<Category>> GetCategoriesWithProducts()
    {
        _logger.LogInformation("##### Getting categories with products #####");
        var categories = _context.Categories.Include(c => c.Products).Where(c => c.CategoryId <= 5).ToList();
        if (categories is null)
        {
            return NotFound();
        }
        return categories;
    } */

    [HttpPost]
    public ActionResult Post(Category category)
    {
        if (category is null)
        {
            return BadRequest("Invalid category data.");
        }
        var newCategory = _repository.AddCategory(category);
        return new CreatedAtRouteResult("GetCategoryById", new { id = newCategory.CategoryId }, newCategory);
    }

    [HttpPut("{id:int:min(1)}")]
    public ActionResult Put(int id, Category category)
    {
        if (id != category.CategoryId)
        {
            return BadRequest("ID mismatch or Invalid category data.");
        }
        var updatedCategory = _repository.UpdateCategory(category);
        return Ok(updatedCategory);
    }

    [HttpDelete("{id:int:min(1)}")]
    public ActionResult Delete(int id)
    {
        var category = _repository.GetCategoryById(id);
        if (category is null)
        {
            return NotFound($"Category {id} not found!");
        }
        var deletedCategory = _repository.DeleteCategory(id);
        return Ok(deletedCategory);
    }

}