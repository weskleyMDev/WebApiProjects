using CatalogoAPI.Context;
using CatalogoAPI.Models;
using CatalogoAPI.Pagination;
using CatalogoAPI.Repositories.Interfaces;

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

    public PagedList<Product> GetProductsByPrice(ProductsFilterPrice productsFilterPrice)
    {
        var products = GetAll().AsQueryable();

        if (productsFilterPrice.Price.HasValue && !string.IsNullOrEmpty(productsFilterPrice.PriceFilter))
        {
            switch (productsFilterPrice.PriceFilter.ToLower())
            {
                case "equals":
                    products = products.Where(p => p.Price == productsFilterPrice.Price).OrderBy(p => p.Price);
                    break;
                case "bigger":
                    products = products.Where(p => p.Price > productsFilterPrice.Price).OrderBy(p => p.Price);
                    break;
                case "smaller":
                    products = products.Where(p => p.Price < productsFilterPrice.Price).OrderBy(p => p.Price);
                    break;
                default:
                    break;
            }
        }

        var filteredProducts = PagedList<Product>.ToPagedList(products, productsFilterPrice.PageNumber, productsFilterPrice.PageSize);
        return filteredProducts;
    }
}