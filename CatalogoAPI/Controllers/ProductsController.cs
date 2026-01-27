using CatalogoAPI.Models;
using CatalogoAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CatalogoAPI.Controllers;

[Route("[controller]")]
[ApiController]
public class ProductsController(IProductRepository repository) : ControllerBase
{
    private readonly IProductRepository _repository = repository;

    [HttpGet]
    public ActionResult<IEnumerable<Product>> GetProductsAsync()
    {
        var products = _repository.GetAll();
        if (products is null)
        {
            return NotFound("No products found!");
        }
        return Ok(products);
    }

    [HttpGet("{id:int:min(1)}", Name = "GetProductById")]
    public ActionResult<Product> GetProductAsync(int id)
    {
        var product = _repository.GetById(p => p.ProductId == id);
        if (product is null)
        {
            return NotFound($"Product {id} not found!");
        }
        return Ok(product);
    }

    [HttpGet("products/{id:int:min(1)}")]
    public ActionResult<IEnumerable<Product>> GetProductsByCategory(int id)
    {
        var products = _repository.GetProductsByCategoryId(id);
        if (products is null)
        {
            return NotFound($"No products found for this category {id}!");
        }
        return Ok(products);
    }

    [HttpPost]
    public ActionResult Post(Product product)
    {
        // if (!ModelState.IsValid)
        // {
        //     return BadRequest(ModelState);
        // }
        if (product is null)
        {
            return BadRequest("Invalid product data.");
        }
        var newProduct = _repository.Add(product);
        return new CreatedAtRouteResult("GetProductById", new { id = newProduct.ProductId }, newProduct);
    }

    [HttpPut("{id:int:min(1)}")]
    public ActionResult Put(int id, Product product)
    {
        if (id != product.ProductId)
        {
            return BadRequest("ID mismatch or Invalid product data.");
        }
        var updatedProduct = _repository.Update(product);
        return Ok(updatedProduct);
    }

    [HttpDelete("{id:int:min(1)}")]
    public ActionResult Delete(int id)
    {
        var product = _repository.GetById(p => p.ProductId == id);
        if (product is null)
        {
            return NotFound($"Product {id} not found!");
        }
        var deletedProduct = _repository.Delete(product);
        return Ok(deletedProduct);
    }
}