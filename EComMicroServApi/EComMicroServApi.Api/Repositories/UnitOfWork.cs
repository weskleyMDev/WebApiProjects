namespace EComMicroServApi.Api.Repositories;

using EComMicroServApi.Api.Data;
using EComMicroServApi.Api.Repositories.Interfaces;
using EComMicroServApi.Api.Services;
using EComMicroServApi.Api.Services.Interfaces;

public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    private readonly AppDbContext _context = context;

    private IProductService? _productService;
    private ICategoryService? _categoryService;
    private IProductRepository? _productRepository;
    private ICategoryRepository? _categoryRepository;

    private IProductRepository ProductRepository =>
        _productRepository ??= new ProductRepository(_context);
    public IProductService ProductService =>
        _productService ??= new ProductService(_context, ProductRepository, CategoryRepository);
    private ICategoryRepository CategoryRepository =>
        _categoryRepository ??= new CategoryRepository(_context);
    public ICategoryService CategoryService =>
        _categoryService ??= new CategoryService(_context, CategoryRepository);

    public void Dispose()
    {
        _context.Dispose();
    }
}