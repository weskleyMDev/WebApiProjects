using CatalogoAPI.Context;
using CatalogoAPI.Models;
using CatalogoAPI.Pagination;
using Microsoft.EntityFrameworkCore;

namespace CatalogoAPI.Repositories;

public class CategoryRepository(AppDbContext context) : Repository<Category>(context), ICategoryRepository
{
    public PagedList<Category> GetCategories(CategoriesParameters categoriesParameters)
    {
        var source = GetAll().OrderBy(c => c.CategoryId).AsQueryable();
        return PagedList<Category>.ToPagedList(source, categoriesParameters.PageNumber, categoriesParameters.PageSize);
    }
}