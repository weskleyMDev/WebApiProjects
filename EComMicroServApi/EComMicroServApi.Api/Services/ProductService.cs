using EComMicroServApi.Api.Data;
using EComMicroServApi.Api.Models;
using EComMicroServApi.Api.Models.DTOs;
using EComMicroServApi.Api.Repositories.Interfaces;
using EComMicroServApi.Api.Services.Interfaces;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace EComMicroServApi.Api.Services;

public class ProductService(AppDbContext context, IProductRepository repository,
    ICategoryRepository categoryRepository) : CrudService<InputProductDto, OutputProductDto, Product, IProductRepository>(context, repository), IProductService
{
    private readonly ICategoryRepository _categoryRepository = categoryRepository;
    public override async Task<OutputProductDto> CreateAsync(InputProductDto entityDto)
    {
        await ValidateCategoryAsync(entityDto.CategoryId);

        var entity = entityDto.Adapt<Product>();

        _repository.Create(entity);

        await _context.SaveChangesAsync();

        var product = await _context.Products
            .Include(p => p.Category)
            .AsNoTracking()
            .FirstAsync(p => p.Id == entity.Id);

        return product.Adapt<OutputProductDto>();
    }

    public override async Task<OutputProductDto?> UpdateAsync(
    int id,
    InputProductDto entityDto)
    {
        await ValidateCategoryAsync(entityDto.CategoryId);

        var result = await base.UpdateAsync(id, entityDto);

        if (result is null)
        {
            return null;
        }

        var product = await _context.Products
            .Include(p => p.Category)
            .AsNoTracking()
            .FirstAsync(p => p.Id == id);

        return product.Adapt<OutputProductDto>();
    }

    private async Task ValidateCategoryAsync(int categoryId)
    {
        var category = await _categoryRepository.GetByIdAsync(categoryId) ?? throw new KeyNotFoundException(
                $"Category with id {categoryId} was not found.");
    }
}