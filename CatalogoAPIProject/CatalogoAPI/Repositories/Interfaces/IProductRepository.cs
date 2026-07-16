using CatalogoAPI.Models;
using CatalogoAPI.Pagination;
using X.PagedList;

namespace CatalogoAPI.Repositories.Interfaces;

public interface IProductRepository : IRepository<Product>
{
    // IEnumerable<Product> GetProducts(ProductsParameters productsParameters);
    Task<IPagedList<Product>> GetProductsAsync(ProductsParameters productsParameters);
    Task<IPagedList<Product>> GetProductsByPriceAsync(ProductsFilterPrice productsFilterPrice);
    Task<IEnumerable<Product>> GetProductsByCategoryIdAsync(int categoryId);
}