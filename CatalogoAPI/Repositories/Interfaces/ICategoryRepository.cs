using CatalogoAPI.Models;
using CatalogoAPI.Pagination;

namespace CatalogoAPI.Repositories.Interfaces;

public interface ICategoryRepository : IRepository<Category>
{
    PagedList<Category> GetCategories(CategoriesParameters categoriesParameters);

    PagedList<Category> GetCategoriesByName(CategoriesFilterName categoriesFilterName);
}