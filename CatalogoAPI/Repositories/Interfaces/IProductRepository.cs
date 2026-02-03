using CatalogoAPI.Models;
using CatalogoAPI.Pagination;

namespace CatalogoAPI.Repositories.Interfaces;

public interface IProductRepository : IRepository<Product>
{
    // IEnumerable<Product> GetProducts(ProductsParameters productsParameters);
    PagedList<Product> GetProducts(ProductsParameters productsParameters);
    PagedList<Product> GetProductsByPrice(ProductsFilterPrice productsFilterPrice);
    IEnumerable<Product> GetProductsByCategoryId(int categoryId);
}