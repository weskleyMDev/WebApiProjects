using CatalogoAPI.Models;
using CatalogoAPI.Pagination;

namespace CatalogoAPI.Repositories;

public interface ICategoryRepository : IRepository<Category>
{
    PagedList<Category> GetCategories(CategoriesParameters categoriesParameters);
}