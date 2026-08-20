using EComMicroServApi.Api.Exceptions;
using EComMicroServApi.Api.Models;
using EComMicroServApi.Api.Models.DTOs;
using EComMicroServApi.Api.Repositories.Interfaces;
using EComMicroServApi.Api.Services.Interfaces;
using Mapster;

namespace EComMicroServApi.Api.Services;

public class ProductService(IProductRepository repository,
    ICategoryRepository categoryRepository) : CrudService<InputProductDto, OutputProductDto, Product, IProductRepository>(repository), IProductService
{
    private readonly ICategoryRepository _categoryRepository = categoryRepository;
    public override async Task<OutputProductDto> CreateAsync(InputProductDto entityDto)
    {
        await ValidateCategoryAsync(entityDto.CategoryId);

        var entity = entityDto.Adapt<Product>();

        _repository.Create(entity);

        await _repository.SaveChangesAsync();

        var product = await _repository.GetByIdAsync(entity.Id) ?? throw new InvalidOperationException(
                $"Product with id {entity.Id} was not found after creation.");
        return product.Adapt<OutputProductDto>();
    }

    public override async Task<OutputProductDto?> UpdateAsync(
    int id,
    InputProductDto entityDto)
    {
        await ValidateCategoryAsync(entityDto.CategoryId);

        return await base.UpdateAsync(id, entityDto);
    }

    private async Task ValidateCategoryAsync(int categoryId)
    {
        var category = await _categoryRepository.GetByIdAsync(categoryId) ?? throw new NotFoundException(
            $"Category with id {categoryId} was not found.");
    }
}