using EComMicroServApi.Api.Models.DTOs;

namespace EComMicroServApi.Api.Services.Interfaces;

public interface ICategoryService : ICrudService<InputCategoryDto, OutputCategoryDto>
{
    Task<IEnumerable<OutputCategoryDto>> GetCategoriesWithProducts();
}