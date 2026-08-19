using EComMicroServApi.Api.Models.DTOs;
using EComMicroServApi.Api.Repositories.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace EComMicroServApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IUnitOfWork unitOfWork) : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        var products = await _unitOfWork.Products.GetAllAsync();
        return Ok(products);
    }

    [HttpGet("{id:int:min(1)}", Name = "GetProduct")]
    public async Task<IActionResult> GetProduct(int id)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct(InputProductDto productDto)
    {
        var product = _unitOfWork.Products.Add(productDto);
        await _unitOfWork.SaveChangesAsync();
        var newProduct = product.Adapt<OutputProductDto>();
        return CreatedAtAction(nameof(GetProduct), new { id = newProduct.Id }, newProduct);
    }
}