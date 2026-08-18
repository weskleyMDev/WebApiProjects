using EComMicroServApi.Api.Data;
using EComMicroServApi.Api.Models;
using EComMicroServApi.Api.Models.DTOs;
using EComMicroServApi.Api.Repositories.Interfaces;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace EComMicroServApi.Api.Repositories;

public class CategoryRepository(AppDbContext context) : Repository<InputCategoryDto, OutputCategoryDto, Category>(context), ICategoryRepository
{
    public async Task<IEnumerable<OutputCategoryDto>> GetCategoriesWithProducts()
    {
        var categories = await _context.Categories
            .AsNoTracking()
            .Include(c => c.Products)
            .ToListAsync();

        return categories.Adapt<IEnumerable<OutputCategoryDto>>();
    }
}