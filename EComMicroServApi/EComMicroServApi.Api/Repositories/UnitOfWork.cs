namespace EComMicroServApi.Api.Repositories;

using EComMicroServApi.Api.Data;
using EComMicroServApi.Api.Repositories.Interfaces;

public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    private readonly AppDbContext _context = context;

    private IProductRepository? _products;
    private ICategoryRepository? _categories;

    public IProductRepository Products =>
        _products ??= new ProductRepository(_context);

    public ICategoryRepository Categories =>
        _categories ??= new CategoryRepository(_context);

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}