using EComMicroServApi.Api.Data;
using EComMicroServApi.Api.Models;
using EComMicroServApi.Api.Models.DTOs;
using EComMicroServApi.Api.Repositories.Interfaces;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace EComMicroServApi.Api.Repositories;

public class ProductRepository(AppDbContext context) : Repository<Product>(context), IProductRepository
{
    public override async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _context.Products
            .Include(p => p.Category)
            .AsNoTracking()
            .ToListAsync();
    }

    public override async Task<Product?> GetByIdAsync(int id)
    {
        return await _context.Products
            .Include(p => p.Category)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public override async Task<Product?> UpdateAsync(int id, Product entity)
    {
        var updatedEntity = await _context.Products
            .FirstOrDefaultAsync(p => p.Id == id);

        if (updatedEntity == null)
        {
            return null;
        }

        updatedEntity.Name = entity.Name;
        updatedEntity.Price = entity.Price;
        updatedEntity.Description = entity.Description;
        updatedEntity.Stock = entity.Stock;
        updatedEntity.ImageUrl = entity.ImageUrl;
        updatedEntity.CategoryId = entity.CategoryId;

        return updatedEntity;
    }
}