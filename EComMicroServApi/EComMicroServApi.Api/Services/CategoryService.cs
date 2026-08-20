using EComMicroServApi.Api.Data;
using EComMicroServApi.Api.Models;
using EComMicroServApi.Api.Models.DTOs;
using EComMicroServApi.Api.Repositories.Interfaces;
using EComMicroServApi.Api.Services.Interfaces;
using Mapster;

namespace EComMicroServApi.Api.Services;

public class CategoryService(ICategoryRepository repository) : CrudService<InputCategoryDto, OutputCategoryDto, Category, ICategoryRepository>(repository), ICategoryService
{
    public async Task<IEnumerable<OutputCategoryDto>> GetCategoriesWithProducts()
    {
        var categories = await _repository.GetCategoriesWithProducts();
        return categories.Adapt<IEnumerable<OutputCategoryDto>>();
    }
}