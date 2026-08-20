using EComMicroServApi.Api.Models.DTOs;
using EComMicroServApi.Api.Repositories.Interfaces;
using EComMicroServApi.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EComMicroServApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class CategoriesController(ICategoryService service) : ControllerBase
{
    private readonly ICategoryService _service = service;

    [HttpGet]
    public async Task<IEnumerable<OutputCategoryDto>> GetCategories()
    {
        return await _service.GetAllAsync();
    }

    [HttpGet("Products")]
    public async Task<IEnumerable<OutputCategoryDto>> GetCategoriesWithProducts()
    {
        return await _service.GetCategoriesWithProducts();
    }

    [HttpGet("{id:int:min(1)}", Name = "GetCategory")]
    public async Task<ActionResult<OutputCategoryDto>> GetCategory(int id)
    {
        var category = await _service.GetByIdAsync(id);
        if (category == null)
        {
            return NotFound();
        }
        return Ok(category);
    }

    [HttpPost]
    public async Task<ActionResult<OutputCategoryDto>> CreateCategory(InputCategoryDto categoryDto)
    {
        var category = await _service.CreateAsync(categoryDto);
        return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
    }

    [HttpPut("{id:int:min(1)}")]
    public async Task<ActionResult<OutputCategoryDto>> UpdateCategory(int id, InputCategoryDto categoryDto)
    {
        var updatedCategory = await _service.UpdateAsync(id, categoryDto);
        if (updatedCategory is null)
        {
            return NotFound();
        }
        return Ok(updatedCategory);
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> RemoveCategory(int id)
    {
        var isDeleted = await _service.DeleteAsync(id);
        if (!isDeleted)
        {
            return NotFound();
        }
        return NoContent();
    }
}