using CatalogoAPI.Context;
using CatalogoAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CatalogoAPI.Controllers;

[Route("[controller]")]
[ApiController]
public class ProductsController(AppDbContext context) : ControllerBase
{
    private readonly AppDbContext _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProductsAsync()
    {
        var products = await _context.Products.AsNoTracking().ToListAsync();
        if (products is null)
        {
            return NotFound();
        }
        return products;
    }

    [HttpGet("{id:int:min(1)}", Name = "GetProductById")]
    public async Task<ActionResult<Product>> GetProductAsync(int id)
    {
        var product = await _context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.ProductId == id);
        if (product is null)
        {
            return NotFound($"Product {id} not found!");
        }
        return product;
    }

    [HttpPost]
    public ActionResult Post(Product product)
    {
        // if (!ModelState.IsValid)
        // {
        //     return BadRequest(ModelState);
        // }
        _context.Products.Add(product);
        _context.SaveChanges();
        return new CreatedAtRouteResult("GetProductById", new { id = product.ProductId }, product);
    }

    [HttpPut("{id:int:min(1)}")]
    public ActionResult Put(int id, Product product)
    {
        if (id != product.ProductId)
        {
            return BadRequest();
        }
        _context.Entry(product).State = EntityState.Modified;
        _context.SaveChanges();
        return Ok(product);
    }

    [HttpDelete("{id:int:min(1)}")]
    public ActionResult Delete(int id)
    {
        var product = _context.Products.FirstOrDefault(p => p.ProductId == id);
        if (product is null)
        {
            return NotFound($"Product {id} not found!");
        }
        _context.Products.Remove(product);
        _context.SaveChanges();
        return Ok(product);
    }
}