using CatalogoAPI.Context;
using CatalogoAPI.Repositories.Interfaces;

namespace CatalogoAPI.Repositories;

public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    private IProductRepository? _productRepository;
    private ICategoryRepository? _categoryRepository;
    public AppDbContext _context = context;
    public IProductRepository ProductRepository => _productRepository ??= new ProductRepository(_context);

    public ICategoryRepository CategoryRepository => _categoryRepository ??= new CategoryRepository(_context);

    public void Commit()
    {
        _context.SaveChanges();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}