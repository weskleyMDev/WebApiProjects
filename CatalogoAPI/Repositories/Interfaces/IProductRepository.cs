using CatalogoAPI.Models;
using CatalogoAPI.Pagination;

namespace CatalogoAPI.Repositories.Interfaces;

public interface IProductRepository : IRepository<Product>
{
    // IEnumerable<Product> GetProducts(ProductsParameters productsParameters);
    Task<PagedList<Product>> GetProductsAsync(ProductsParameters productsParameters);
    Task<PagedList<Product>> GetProductsByPriceAsync(ProductsFilterPrice productsFilterPrice);
    Task<IEnumerable<Product>> GetProductsByCategoryIdAsync(int categoryId);
}