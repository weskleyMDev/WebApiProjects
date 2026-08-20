using EComMicroServApi.Api.Models.DTOs;
using EComMicroServApi.Api.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EComMicroServApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ProductsController(IUnitOfWork unitOfWork) : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    [HttpGet]
    public async Task<IEnumerable<OutputProductDto>> GetProducts()
    {
        return await _unitOfWork.ProductService.GetAllAsync();
    }

    [HttpGet("{id:int:min(1)}", Name = "GetProduct")]
    public async Task<ActionResult<OutputProductDto>> GetProduct(int id)
    {
        var product = await _unitOfWork.ProductService.GetByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        return Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult<OutputProductDto>> CreateProduct(InputProductDto productDto)
    {
        var product = await _unitOfWork.ProductService.CreateAsync(productDto);
        return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
    }

    [HttpPut("{id:int:min(1)}")]
    public async Task<ActionResult<OutputProductDto>> UpdateProduct(int id, InputProductDto productDto)
    {
        var updatedProduct = await _unitOfWork.ProductService.UpdateAsync(id, productDto);
        if (updatedProduct is null)
        {
            return NotFound();
        }
        return Ok(updatedProduct);
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> RemoveProduct(int id)
    {
        var isDeleted = await _unitOfWork.ProductService.DeleteAsync(id);
        if (!isDeleted)
        {
            return NotFound();
        }
        return NoContent();
    }
}