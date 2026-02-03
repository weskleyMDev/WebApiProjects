using CatalogoAPI.Context;
using CatalogoAPI.Models;
using CatalogoAPI.Pagination;
using CatalogoAPI.Repositories.Interfaces;

namespace CatalogoAPI.Repositories;

public class CategoryRepository(AppDbContext context) : Repository<Category>(context), ICategoryRepository
{
    public PagedList<Category> GetCategories(CategoriesParameters categoriesParameters)
    {
        var source = GetAll().OrderBy(c => c.CategoryId).AsQueryable();
        return PagedList<Category>.ToPagedList(source, categoriesParameters.PageNumber, categoriesParameters.PageSize);
    }

    public PagedList<Category> GetCategoriesByName(CategoriesFilterName categoriesFilterName)
    {
        var categories = GetAll().AsQueryable();
        if (!string.IsNullOrEmpty(categoriesFilterName.Name))
        {
            categories = categories.Where(c => c.Name != null && c.Name.Contains(categoriesFilterName.Name, StringComparison.CurrentCultureIgnoreCase));
        }
        return PagedList<Category>.ToPagedList(categories, categoriesFilterName.PageNumber, categoriesFilterName.PageSize);
    }
}