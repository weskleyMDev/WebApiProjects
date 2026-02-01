using CatalogoAPI.DTOs;
using CatalogoAPI.DTOs.Mappings;
using CatalogoAPI.Filter;
using CatalogoAPI.Models;
using CatalogoAPI.Pagination;
using CatalogoAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CatalogoAPI.Controllers;

[Route("[controller]")]
[ApiController]
public class CategoriesController(IUnitOfWork unitOfWork, IConfiguration configuration, ILogger<CategoriesController> logger) : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
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
    public ActionResult<IEnumerable<CategoryDTO>> Get()
    {
        var categories = _unitOfWork.CategoryRepository.GetAll();
        if (categories is null)
        {
            return NotFound("No categories found!");
        }

        var categoriesDTO = categories.ToDTOs();

        return Ok(categoriesDTO);
    }

    [HttpGet("paginated")]
    public ActionResult<IEnumerable<CategoryDTO>> GetPaginated([FromQuery] CategoriesParameters categoriesParameters)
    {
        var categories = _unitOfWork.CategoryRepository.GetCategories(categoriesParameters);
        if (categories is null)
        {
            return NotFound("No categories found!");
        }
        var metadata = new
        {
            categories.TotalCount,
            categories.PageSize,
            categories.CurrentPage,
            categories.TotalPages,
            categories.HasNext,
            categories.HasPrevious
        };
        Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));
        var categoriesDTO = categories.ToDTOs();
        return Ok(categoriesDTO);
    }

    [HttpGet("{id:int:min(1)}", Name = "GetCategoryById")]
    public ActionResult<CategoryDTO> Get(int id)
    {
        var category = _unitOfWork.CategoryRepository.GetById(c => c.CategoryId == id);
        if (category is null)
        {
            return NotFound($"Category {id} not found!");
        }

        var categoryDTO = category.ToDTO();

        return Ok(categoryDTO);
    }


    [HttpPost]
    public ActionResult<CategoryDTO> Post(CategoryDTO categoryDTO)
    {
        if (categoryDTO is null)
        {
            return BadRequest("Invalid category data.");
        }
        var category = categoryDTO.ToEntity();

        var newCategory = _unitOfWork.CategoryRepository.Add(category!);
        _unitOfWork.Commit();

        var newCategoryDTO = newCategory.ToDTO();

        return new CreatedAtRouteResult("GetCategoryById", new { id = newCategoryDTO!.CategoryId }, newCategoryDTO);
    }

    [HttpPut("{id:int:min(1)}")]
    public ActionResult<CategoryDTO> Put(int id, CategoryDTO categoryDTO)
    {
        if (id != categoryDTO.CategoryId)
        {
            return BadRequest("ID mismatch or Invalid category data.");
        }

        var category = categoryDTO.ToEntity();

        var updatedCategory = _unitOfWork.CategoryRepository.Update(category!);
        _unitOfWork.Commit();

        var updatedCategoryDTO = updatedCategory.ToDTO();

        return Ok(updatedCategoryDTO);
    }

    [HttpDelete("{id:int:min(1)}")]
    public ActionResult<CategoryDTO> Delete(int id)
    {
        var category = _unitOfWork.CategoryRepository.GetById(c => c.CategoryId == id);
        if (category is null)
        {
            return NotFound($"Category {id} not found!");
        }
        var deletedCategory = _unitOfWork.CategoryRepository.Delete(category);
        _unitOfWork.Commit();

        var deletedCategoryDTO = deletedCategory.ToDTO();
        return Ok(deletedCategoryDTO);
    }

}