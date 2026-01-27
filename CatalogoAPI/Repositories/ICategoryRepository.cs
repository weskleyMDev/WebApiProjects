using CatalogoAPI.Models;

namespace CatalogoAPI.Repositories;

public interface ICategoryRepository
{
    IEnumerable<Category> GetCategories();
    Category? GetCategoryById(int id);
    Category AddCategory(Category category);
    Category UpdateCategory(Category category);
    Category DeleteCategory(int id);
}