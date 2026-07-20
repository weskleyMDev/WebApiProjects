using CategoryMVC.Models;

namespace CategoryMVC.Services;

public interface ICategoryService
{
    Task<IEnumerable<CategoryViewModel>?> GetCategories();
    Task<CategoryViewModel?> GetCategoryById(int id);
    Task<CategoryViewModel?> CreateCategory(CategoryViewModel categoryView);
    Task<bool> UpdateCategory(int id, CategoryViewModel categoryView);
    Task<bool> RemoveCategory(int id);
}