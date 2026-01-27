using CatalogoAPI.Context;
using CatalogoAPI.Models;

namespace CatalogoAPI.Repositories;

public class ProductRepository(AppDbContext context) : Repository<Product>(context), IProductRepository
{
    public IEnumerable<Product> GetProductsByCategoryId(int categoryId)
    {
        return GetAll().Where(p => p.CategoryId == categoryId);
    }
}