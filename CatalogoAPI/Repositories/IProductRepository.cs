using CatalogoAPI.Models;
using CatalogoAPI.Pagination;

namespace CatalogoAPI.Repositories;

public interface IProductRepository : IRepository<Product>
{
    // IEnumerable<Product> GetProducts(ProductsParameters productsParameters);
    PagedList<Product> GetProducts(ProductsParameters productsParameters);
    IEnumerable<Product> GetProductsByCategoryId(int categoryId);
}