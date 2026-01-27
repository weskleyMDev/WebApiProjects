using CatalogoAPI.Context;
using CatalogoAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogoAPI.Repositories;

public class CategoryRepository(AppDbContext context) : ICategoryRepository
{
    private readonly AppDbContext _context = context;

    public Category AddCategory(Category category)
    {
        ArgumentNullException.ThrowIfNull(category);

        _context.Categories.Add(category);
        _context.SaveChanges();
        return category;
    }

    public Category DeleteCategory(int id)
    {
        var category = _context.Categories.Find(id);
        ArgumentNullException.ThrowIfNull(category);

        _context.Categories.Remove(category);
        _context.SaveChanges();
        return category;
    }

    public IEnumerable<Category> GetCategories()
    {
        return [.. _context.Categories];
    }

    public Category? GetCategoryById(int id)
    {
        return _context.Categories.FirstOrDefault(c => c.CategoryId == id);
    }

    public Category UpdateCategory(Category category)
    {
        ArgumentNullException.ThrowIfNull(category);

        _context.Entry(category).State = EntityState.Modified;
        _context.SaveChanges();
        return category;
    }
}