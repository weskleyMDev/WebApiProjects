using EComMicroServApi.Api.Models.DTOs;
using EComMicroServApi.Api.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EComMicroServApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class CategoriesController(IUnitOfWork unitOfWork) : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    [HttpGet]
    public async Task<IEnumerable<OutputCategoryDto>> GetCategories()
    {
        return await _unitOfWork.CategoryService.GetAllAsync();
    }

    [HttpGet("Products")]
    public async Task<IEnumerable<OutputCategoryDto>> GetCategoriesWithProducts()
    {
        return await _unitOfWork.CategoryService.GetCategoriesWithProducts();
    }

    [HttpGet("{id:int:min(1)}", Name = "GetCategory")]
    public async Task<ActionResult<OutputCategoryDto>> GetCategory(int id)
    {
        var category = await _unitOfWork.CategoryService.GetByIdAsync(id);
        if (category == null)
        {
            return NotFound();
        }
        return Ok(category);
    }

    [HttpPost]
    public async Task<ActionResult<OutputCategoryDto>> CreateCategory(InputCategoryDto categoryDto)
    {
        var category = await _unitOfWork.CategoryService.CreateAsync(categoryDto);
        return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
    }

    [HttpPut("{id:int:min(1)}")]
    public async Task<ActionResult<OutputCategoryDto>> UpdateCategory(int id, InputCategoryDto categoryDto)
    {
        var updatedCategory = await _unitOfWork.CategoryService.UpdateAsync(id, categoryDto);
        if (updatedCategory is null)
        {
            return NotFound();
        }
        return Ok(updatedCategory);
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> RemoveCategory(int id)
    {
        var isDeleted = await _unitOfWork.CategoryService.DeleteAsync(id);
        if (!isDeleted)
        {
            return NotFound();
        }
        return NoContent();
    }
}