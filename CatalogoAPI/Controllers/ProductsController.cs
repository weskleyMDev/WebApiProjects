using CatalogoAPI.Context;
using CatalogoAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CatalogoAPI.Controllers;

[Route("[controller]")]
[ApiController]
public class ProductsController(AppDbContext context) : ControllerBase
{
    private readonly AppDbContext _context = context;

    [HttpGet]
    public ActionResult<IEnumerable<Product>> Get()
    {
        var products = _context.Products.Take(10).ToList();
        if (products is null)
        {
            return NotFound();
        }
        return products;
    }

    [HttpGet("{id:int}", Name = "GetProductById")]
    public ActionResult<Product> Get(int id)
    {
        var product = _context.Products.FirstOrDefault(p => p.ProductId == id);
        if (product is null)
        {
            return NotFound("Product not found!");
        }
        return product;
    }

    [HttpPost]
    public ActionResult Post(Product product)
    {
        if (product is null)
        {
            return BadRequest();
        }
        _context.Products.Add(product);
        _context.SaveChanges();
        return new CreatedAtRouteResult("GetProductById", new { id = product.ProductId }, product);
    }

    [HttpPut("{id:int}")]
    public ActionResult Put(int id, Product product)
    {
        if (id != product.ProductId)
        {
            return BadRequest();
        }
        _context.Entry(product).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        _context.SaveChanges();
        return Ok(product);
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        var product = _context.Products.FirstOrDefault(p => p.ProductId == id);
        if (product is null)
        {
            return NotFound("Product not found!");
        }
        _context.Products.Remove(product);
        _context.SaveChanges();
        return Ok(product);
    }
}