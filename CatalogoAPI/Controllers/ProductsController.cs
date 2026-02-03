using AutoMapper;
using CatalogoAPI.DTOs;
using CatalogoAPI.Models;
using CatalogoAPI.Pagination;
using CatalogoAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CatalogoAPI.Controllers;

[Route("[controller]")]
[ApiController]
public class ProductsController(IUnitOfWork unitOfWork, IMapper mapper) : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    [HttpGet("paginated")]
    public ActionResult<IEnumerable<ProductDTO>> GetProductsPaginated([FromQuery] ProductsParameters productsParameters)
    {
        var products = _unitOfWork.ProductRepository.GetProducts(productsParameters);
        if (products is null)
        {
            return NotFound("No products found!");
        }
        
        return GetProductsDTO(products);
    }

    [HttpGet("filter/price/paginated")]
    public ActionResult<IEnumerable<ProductDTO>> GetProductsByPrice([FromQuery] ProductsFilterPrice productsFilterPrice)
    {
        var products = _unitOfWork.ProductRepository.GetProductsByPrice(productsFilterPrice);
        if (products is null)
        {
            return NotFound("No products found!");
        }
        return GetProductsDTO(products);
    }

    private ActionResult<IEnumerable<ProductDTO>> GetProductsDTO(PagedList<Product> products)
    {
        var metadata = new
        {
            products.TotalCount,
            products.PageSize,
            products.CurrentPage,
            products.TotalPages,
            products.HasNext,
            products.HasPrevious
        };
        Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));
        var productsDTO = _mapper.Map<IEnumerable<ProductDTO>>(products);
        return Ok(productsDTO);
    }

    [HttpGet]
    public ActionResult<IEnumerable<ProductDTO>> GetProducts()
    {
        var products = _unitOfWork.ProductRepository.GetAll();
        if (products is null)
        {
            return NotFound("No products found!");
        }
        var productsDTO = _mapper.Map<IEnumerable<ProductDTO>>(products);
        return Ok(productsDTO);
    }

    [HttpGet("{id:int:min(1)}", Name = "GetProductById")]
    public ActionResult<ProductDTO> GetProduct(int id)
    {
        var product = _unitOfWork.ProductRepository.GetById(p => p.ProductId == id);
        if (product is null)
        {
            return NotFound($"Product {id} not found!");
        }
        var productDTO = _mapper.Map<ProductDTO>(product);
        return Ok(productDTO);
    }

    [HttpGet("products/{id:int:min(1)}")]
    public ActionResult<IEnumerable<ProductDTO>> GetProductsByCategory(int id)
    {
        var products = _unitOfWork.ProductRepository.GetProductsByCategoryId(id);
        if (products is null)
        {
            return NotFound($"No products found for this category {id}!");
        }
        var productsDTO = _mapper.Map<IEnumerable<ProductDTO>>(products);
        return Ok(productsDTO);
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
        var product = _mapper.Map<Product>(productDTO);
        var newProduct = _unitOfWork.ProductRepository.Add(product);
        _unitOfWork.Commit();
        var newProductDTO = _mapper.Map<ProductDTO>(newProduct);
        return new CreatedAtRouteResult("GetProductById", new { id = newProductDTO.ProductId }, newProductDTO);
    }

    [HttpPatch("{id:int:min(1)}/updatePartial")]
    public ActionResult<ProductDTOResponse> Patch(int id, JsonPatchDocument<ProductDTOUpdate> pathProductDTO)
    {
        if (pathProductDTO is null || id <= 0)
        {
            return BadRequest("Invalid product data/id.");
        }
        var product = _unitOfWork.ProductRepository.GetById(p => p.ProductId == id);
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
        _unitOfWork.Commit();
        var updatedProductDTOResponse = _mapper.Map<ProductDTOResponse>(updatedProduct);
        return Ok(updatedProductDTOResponse);
    }

    [HttpPut("{id:int:min(1)}")]
    public ActionResult<ProductDTO> Put(int id, ProductDTO productDTO)
    {
        if (id != productDTO.ProductId)
        {
            return BadRequest("ID mismatch or Invalid product data.");
        }
        var product = _mapper.Map<Product>(productDTO);
        var updatedProduct = _unitOfWork.ProductRepository.Update(product);
        _unitOfWork.Commit();
        var updatedProductDTO = _mapper.Map<ProductDTO>(updatedProduct);
        return Ok(updatedProductDTO);
    }

    [HttpDelete("{id:int:min(1)}")]
    public ActionResult<ProductDTO> Delete(int id)
    {
        var product = _unitOfWork.ProductRepository.GetById(p => p.ProductId == id);
        if (product is null)
        {
            return NotFound($"Product {id} not found!");
        }
        var deletedProduct = _unitOfWork.ProductRepository.Delete(product);
        _unitOfWork.Commit();
        var deletedProductDTO = _mapper.Map<ProductDTO>(deletedProduct);
        return Ok(deletedProductDTO);
    }
}