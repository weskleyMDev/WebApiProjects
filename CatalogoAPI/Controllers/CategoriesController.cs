using CatalogoAPI.Filter;
using CatalogoAPI.Models;
using CatalogoAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CatalogoAPI.Controllers;

[Route("[controller]")]
[ApiController]
public class CategoriesController(IRepository<Category> repository, IConfiguration configuration, ILogger<CategoriesController> logger) : ControllerBase
{
    private readonly IRepository<Category> _repository = repository;
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
        var categories = _repository.GetAll();
        if (categories is null)
        {
            return NotFound("No categories found!");
        }
        return Ok(categories);
    }

    [HttpGet("{id:int:min(1)}", Name = "GetCategoryById")]
    public ActionResult<Category> Get(int id)
    {
        var category = _repository.GetById(c => c.CategoryId == id);
        if (category is null)
        {
            return NotFound($"Category {id} not found!");
        }
        return Ok(category);
    }
    

    [HttpPost]
    public ActionResult Post(Category category)
    {
        if (category is null)
        {
            return BadRequest("Invalid category data.");
        }
        var newCategory = _repository.Add(category);
        return new CreatedAtRouteResult("GetCategoryById", new { id = newCategory.CategoryId }, newCategory);
    }

    [HttpPut("{id:int:min(1)}")]
    public ActionResult Put(int id, Category category)
    {
        if (id != category.CategoryId)
        {
            return BadRequest("ID mismatch or Invalid category data.");
        }
        var updatedCategory = _repository.Update(category);
        return Ok(updatedCategory);
    }

    [HttpDelete("{id:int:min(1)}")]
    public ActionResult Delete(int id)
    {
        var category = _repository.GetById(c => c.CategoryId == id);
        if (category is null)
        {
            return NotFound($"Category {id} not found!");
        }
        var deletedCategory = _repository.Delete(category);
        return Ok(deletedCategory);
    }

}