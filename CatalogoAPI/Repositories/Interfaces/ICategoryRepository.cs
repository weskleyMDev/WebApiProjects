using CatalogoAPI.Models;
using CatalogoAPI.Pagination;
using X.PagedList;

namespace CatalogoAPI.Repositories.Interfaces;

public interface ICategoryRepository : IRepository<Category>
{
    Task<IPagedList<Category>> GetCategoriesAsync(CategoriesParameters categoriesParameters);

    Task<IPagedList<Category>> GetCategoriesByNameAsync(CategoriesFilterName categoriesFilterName);
}