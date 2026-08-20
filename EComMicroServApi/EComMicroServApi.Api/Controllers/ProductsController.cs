using EComMicroServApi.Api.Models.DTOs;
using EComMicroServApi.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EComMicroServApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ProductsController(IProductService service) : ControllerBase
{
    private readonly IProductService _service = service;

    [HttpGet]
    public async Task<IEnumerable<OutputProductDto>> GetProducts()
    {
        return await _service.GetAllAsync();
    }

    [HttpGet("{id:int:min(1)}", Name = "GetProduct")]
    public async Task<ActionResult<OutputProductDto>> GetProduct(int id)
    {
        var product = await _service.GetByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        return Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult<OutputProductDto>> CreateProduct(InputProductDto productDto)
    {
        var product = await _service.CreateAsync(productDto);
        return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
    }

    [HttpPut("{id:int:min(1)}")]
    public async Task<ActionResult<OutputProductDto>> UpdateProduct(int id, InputProductDto productDto)
    {
        var updatedProduct = await _service.UpdateAsync(id, productDto);
        if (updatedProduct is null)
        {
            return NotFound();
        }
        return Ok(updatedProduct);
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> RemoveProduct(int id)
    {
        var isDeleted = await _service.DeleteAsync(id);
        if (!isDeleted)
        {
            return NotFound();
        }
        return NoContent();
    }
}