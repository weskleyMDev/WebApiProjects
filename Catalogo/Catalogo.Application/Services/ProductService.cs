using AutoMapper;
using Catalogo.Application.DTOs;
using Catalogo.Application.Interfaces;
using Catalogo.Domain.Interfaces;

namespace Catalogo.Application.Services;

public class ProductService(IProductRepository productRepository, IMapper mapper) : IProductService
{
    private readonly IProductRepository _productRepository = productRepository;
    private readonly IMapper _mapper = mapper;

    public Task Add(ProductDTO productDto)
    {
        throw new NotImplementedException();
    }

    public async Task<ProductDTO> GetProductById(int? id)
    {
        var product = await _productRepository.GetProductByIdAsync(id);
        return _mapper.Map<ProductDTO>(product);
    }

    public async Task<IEnumerable<ProductDTO>> GetProducts()
    {
        var products = await _productRepository.GetProductsAsync();
        return _mapper.Map<IEnumerable<ProductDTO>>(products);
    }

    public Task Remove(int? id)
    {
        throw new NotImplementedException();
    }

    public Task Update(ProductDTO productDto)
    {
        throw new NotImplementedException();
    }
}