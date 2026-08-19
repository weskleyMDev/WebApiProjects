using EComMicroServApi.Api.Data;
using EComMicroServApi.Api.Models;
using EComMicroServApi.Api.Models.DTOs;
using EComMicroServApi.Api.Repositories.Interfaces;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace EComMicroServApi.Api.Repositories;

public class CategoryRepository(AppDbContext context) : Repository<Category>(context), ICategoryRepository
{
    public async Task<IEnumerable<Category>> GetCategoriesWithProducts()
    {
        var categories = await _context.Categories
            .AsNoTracking()
            .Include(c => c.Products)
            .ToListAsync();

        return categories;
    }
}