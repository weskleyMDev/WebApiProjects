using CatalogoAPI.Models;
using CatalogoAPI.Pagination;

namespace CatalogoAPI.Repositories.Interfaces;

public interface ICategoryRepository : IRepository<Category>
{
    Task<PagedList<Category>> GetCategoriesAsync(CategoriesParameters categoriesParameters);

    Task<PagedList<Category>> GetCategoriesByNameAsync(CategoriesFilterName categoriesFilterName);
}