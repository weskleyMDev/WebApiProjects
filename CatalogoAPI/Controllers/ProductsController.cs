using CatalogoAPI.DTOs;
using CatalogoAPI.Models;
using CatalogoAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CatalogoAPI.Controllers;

[Route("[controller]")]
[ApiController]
public class ProductsController(IUnitOfWork unitOfWork) : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    [HttpGet]
    public ActionResult<IEnumerable<ProductDTO>> GetProducts()
    {
        var products = _unitOfWork.ProductRepository.GetAll();
        if (products is null)
        {
            return NotFound("No products found!");
        }
        return Ok(products);
    }

    [HttpGet("{id:int:min(1)}", Name = "GetProductById")]
    public ActionResult<ProductDTO> GetProduct(int id)
    {
        var product = _unitOfWork.ProductRepository.GetById(p => p.ProductId == id);
        if (product is null)
        {
            return NotFound($"Product {id} not found!");
        }
        return Ok(product);
    }

    [HttpGet("products/{id:int:min(1)}")]
    public ActionResult<IEnumerable<ProductDTO>> GetProductsByCategory(int id)
    {
        var products = _unitOfWork.ProductRepository.GetProductsByCategoryId(id);
        if (products is null)
        {
            return NotFound($"No products found for this category {id}!");
        }
        return Ok(products);
    }

    [HttpPost]
    public ActionResult<ProductDTO> Post(ProductDTO productDTO)
    {
        // if (!ModelState.IsValid)
        // {
        //     return BadRequest(ModelState);
        // }
        if (productDTO is null)
        {
            return BadRequest("Invalid product data.");
        }
        var newProduct = _unitOfWork.ProductRepository.Add(product);
        _unitOfWork.Commit();
        return new CreatedAtRouteResult("GetProductById", new { id = newProduct.ProductId }, newProduct);
    }

    [HttpPut("{id:int:min(1)}")]
    public ActionResult<ProductDTO> Put(int id, ProductDTO productDTO)
    {
        if (id != productDTO.ProductId)
        {
            return BadRequest("ID mismatch or Invalid product data.");
        }
        var updatedProduct = _unitOfWork.ProductRepository.Update(product);
        _unitOfWork.Commit();
        return Ok(updatedProduct);
    }

    [HttpDelete("{id:int:min(1)}")]
    public ActionResult Delete(int id)
    {
        var product = _unitOfWork.ProductRepository.GetById(p => p.ProductId == id);
        if (product is null)
        {
            return NotFound($"Product {id} not found!");
        }
        var deletedProduct = _unitOfWork.ProductRepository.Delete(product);
        _unitOfWork.Commit();
        return Ok(deletedProduct);
    }
}