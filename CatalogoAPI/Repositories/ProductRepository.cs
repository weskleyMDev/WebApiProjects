using CatalogoAPI.Context;
using CatalogoAPI.Models;
using CatalogoAPI.Pagination;

namespace CatalogoAPI.Repositories;

public class ProductRepository(AppDbContext context) : Repository<Product>(context), IProductRepository
{
    /* public IEnumerable<Product> GetProducts(ProductsParameters productsParameters)
    {
        return [.. GetAll().OrderBy(p => p.Name).Skip((productsParameters.PageNumber - 1) * productsParameters.PageSize).Take(productsParameters.PageSize)];
    } */

    public PagedList<Product> GetProducts(ProductsParameters productsParameters)
    {
        var source = GetAll().OrderBy(p => p.ProductId).AsQueryable();
        return PagedList<Product>.ToPagedList(source, productsParameters.PageNumber, productsParameters.PageSize);
    }    

    public IEnumerable<Product> GetProductsByCategoryId(int categoryId)
    {
        return GetAll().Where(p => p.CategoryId == categoryId);
    }
}