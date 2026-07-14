using AutoMapper;
using CatalogoAPI.DTOs;
using CatalogoAPI.Models;
using CatalogoAPI.Pagination;
using CatalogoAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using X.PagedList;

namespace CatalogoAPI.Controllers;

[Route("[controller]")]
[ApiController]
// ignore this controller in Swagger UI
// [ApiExplorerSettings(IgnoreApi = true)]
public class ProductsController(IUnitOfWork unitOfWork, IMapper mapper) : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    [HttpGet("paginated")]
    public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductsPaginated([FromQuery] ProductsParameters productsParameters)
    {
        var products = await _unitOfWork.ProductRepository.GetProductsAsync(productsParameters);
        if (products is null)
        {
            return NotFound("No products found!");
        }

        return GetProductsDTO(products);
    }

    [HttpGet("filter/price/paginated")]
    public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductsByPrice([FromQuery] ProductsFilterPrice productsFilterPrice)
    {
        var products = await _unitOfWork.ProductRepository.GetProductsByPriceAsync(productsFilterPrice);
        if (products is null)
        {
            return NotFound("No products found!");
        }
        return GetProductsDTO(products);
    }

    private ActionResult<IEnumerable<ProductDTO>> GetProductsDTO(IPagedList<Product> products)
    {
        var metadata = new
        {
            products.Count,
            products.PageSize,
            products.PageCount,
            products.TotalItemCount,
            products.HasNextPage,
            products.HasPreviousPage
        };
        Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));
        var productsDTO = _mapper.Map<IEnumerable<ProductDTO>>(products);
        return Ok(productsDTO);
    }

    /// <summary>
    /// Get all products
    /// </summary>
    /// <returns>A list of products</returns>
    [HttpGet]
    [Authorize(Policy = "UserOnly")]
    public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts()
    {
        var products = await _unitOfWork.ProductRepository.GetAllAsync();
        if (products is null)
        {
            return NotFound("No products found!");
        }
        var productsDTO = _mapper.Map<IEnumerable<ProductDTO>>(products);
        return Ok(productsDTO);
    }

    /// <summary>
    /// Get a product by its ID
    /// </summary>
    /// <param name="id">Product ID</param>
    /// <returns>A product with the specified ID</returns>
    [HttpGet("{id:int:min(1)}", Name = "GetProductById")]
    public async Task<ActionResult<ProductDTO>> GetProduct(int id)
    {
        var product = await _unitOfWork.ProductRepository.GetByIdAsync(p => p.ProductId == id);
        if (product is null)
        {
            return NotFound($"Product {id} not found!");
        }
        var productDTO = _mapper.Map<ProductDTO>(product);
        return Ok(productDTO);
    }

    [HttpGet("products/{id:int:min(1)}")]
    public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductsByCategory(int id)
    {
        var products = await _unitOfWork.ProductRepository.GetProductsByCategoryIdAsync(id);
        if (products is null)
        {
            return NotFound($"No products found for this category {id}!");
        }
        var productsDTO = _mapper.Map<IEnumerable<ProductDTO>>(products);
        return Ok(productsDTO);
    }

    [HttpPost]
    public async Task<ActionResult<ProductDTO>> Post(ProductDTO productDTO)
    {
        // if (!ModelState.IsValid)
        // {
        //     return BadRequest(ModelState);
        // }
        if (productDTO is null)
        {
            return BadRequest("Invalid product data.");
        }
        var product = _mapper.Map<Product>(productDTO);
        var newProduct = _unitOfWork.ProductRepository.Add(product);
        await _unitOfWork.CommitAsync();
        var newProductDTO = _mapper.Map<ProductDTO>(newProduct);
        return new CreatedAtRouteResult("GetProductById", new { id = newProductDTO.ProductId }, newProductDTO);
    }

    [HttpPatch("{id:int:min(1)}/updatePartial")]
    public async Task<ActionResult<ProductDTOResponse>> Patch(int id, JsonPatchDocument<ProductDTOUpdate> pathProductDTO)
    {
        if (pathProductDTO is null || id <= 0)
        {
            return BadRequest("Invalid product data/id.");
        }
        var product = await _unitOfWork.ProductRepository.GetByIdAsync(p => p.ProductId == id);
        if (product is null)
        {
            return NotFound($"Product {id} not found!");
        }
        var updatedProductDTO = _mapper.Map<ProductDTOUpdate>(product);
        pathProductDTO.ApplyTo(updatedProductDTO, ModelState);

        if (!TryValidateModel(updatedProductDTO))
        {
            return BadRequest(ModelState);
        }
        _mapper.Map(updatedProductDTO, product);
        var updatedProduct = _unitOfWork.ProductRepository.Update(product);
        await _unitOfWork.CommitAsync();
        var updatedProductDTOResponse = _mapper.Map<ProductDTOResponse>(updatedProduct);
        return Ok(updatedProductDTOResponse);
    }

    [HttpPut("{id:int:min(1)}")]
    public async Task<ActionResult<ProductDTO>> Put(int id, ProductDTO productDTO)
    {
        if (id != productDTO.ProductId)
        {
            return BadRequest("ID mismatch or Invalid product data.");
        }
        var product = _mapper.Map<Product>(productDTO);
        var updatedProduct = _unitOfWork.ProductRepository.Update(product);
        await _unitOfWork.CommitAsync();
        var updatedProductDTO = _mapper.Map<ProductDTO>(updatedProduct);
        return Ok(updatedProductDTO);
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<ActionResult<ProductDTO>> Delete(int id)
    {
        var product = await _unitOfWork.ProductRepository.GetByIdAsync(p => p.ProductId == id);
        if (product is null)
        {
            return NotFound($"Product {id} not found!");
        }
        var deletedProduct = _unitOfWork.ProductRepository.Delete(product);
        await _unitOfWork.CommitAsync();
        var deletedProductDTO = _mapper.Map<ProductDTO>(deletedProduct);
        return Ok(deletedProductDTO);
    }
}