using System.Linq.Expressions;
using CatalogoAPI.Context;
using CatalogoAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CatalogoAPI.Repositories;

public class Repository<T>(AppDbContext context) : IRepository<T> where T : class
{
    protected readonly AppDbContext _context = context;

    public T Add(T entity)
    {
        _context.Set<T>().Add(entity);
        // _context.SaveChanges();
        return entity;
    }

    public T Delete(T entity)
    {
        _context.Set<T>().Remove(entity);
        // _context.SaveChanges();
        return entity;
    }

    public IEnumerable<T> GetAll()
    {
        return [.. _context.Set<T>().AsNoTracking()];
    }

    public T? GetById(Expression<Func<T, bool>> predicate)
    {
        return _context.Set<T>().FirstOrDefault(predicate);
    }

    public T Update(T entity)
    {
        _context.Set<T>().Update(entity);
        // _context.SaveChanges();
        return entity;
    }
}