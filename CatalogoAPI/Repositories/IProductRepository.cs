using CatalogoAPI.Models;

namespace CatalogoAPI.Repositories;

public interface IProductRepository : IRepository<Product>
{
    IEnumerable<Product> GetProductsByCategoryId(int categoryId);
}