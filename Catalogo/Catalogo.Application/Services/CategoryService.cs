using AutoMapper;
using Catalogo.Application.DTOs;
using Catalogo.Application.Interfaces;
using Catalogo.Domain.Interfaces;

namespace Catalogo.Application.Services;

public class CategoryService(ICategoryRepository categoryRepository, IMapper mapper) : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository = categoryRepository;
    private readonly IMapper _mapper = mapper;
    
    public Task Add(CategoryDTO categoryDto)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<CategoryDTO>> GetCategories()
    {
        throw new NotImplementedException();
    }

    public Task<CategoryDTO> GetCategoryById(int? id)
    {
        throw new NotImplementedException();
    }

    public Task Remove(int? id)
    {
        throw new NotImplementedException();
    }

    public Task Update(CategoryDTO categoryDto)
    {
        throw new NotImplementedException();
    }
}