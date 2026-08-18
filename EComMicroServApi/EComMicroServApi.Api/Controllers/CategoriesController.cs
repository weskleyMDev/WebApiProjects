using EComMicroServApi.Api.Models.DTOs;
using EComMicroServApi.Api.Repositories.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace EComMicroServApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController(IUnitOfWork unitOfWork) : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    [HttpGet]
    public async Task<IActionResult> GetCategories()
    {
        var categories = await _unitOfWork.Categories.GetCategoriesWithProducts();
        return Ok(categories);
    }

    [HttpGet("{id:int:min(1)}", Name = "GetCategory")]
    public async Task<IActionResult> GetCategory(int id)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(id);
        if (category == null)
        {
            return NotFound();
        }
        return Ok(category);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory(InputCategoryDto categoryDto)
    {
        var category = _unitOfWork.Categories.Add(categoryDto);
        await _unitOfWork.SaveChangesAsync();
        var newCAtegory = category.Adapt<OutputCategoryDto>();
        return CreatedAtAction(nameof(GetCategories), new { id = newCAtegory.Id }, newCAtegory);
    }
}