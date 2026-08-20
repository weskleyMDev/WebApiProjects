using EComMicroServApi.Api.Data;
using EComMicroServApi.Api.Models;
using EComMicroServApi.Api.Repositories.Interfaces;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace EComMicroServApi.Api.Repositories;

public class Repository<E>(AppDbContext context) : IRepository<E> where E : class, IEntity
{
    protected readonly AppDbContext _context = context;

    public E Create(E entity)
    {
        _context.Set<E>().Add(entity);
        return entity;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.Set<E>().FindAsync(id);
        if (entity == null)
        {
            return false;
        }
        _context.Set<E>().Remove(entity);
        return true;
    }

    public virtual async Task<IEnumerable<E>> GetAllAsync()
    {
        return await _context.Set<E>().AsNoTracking().ToListAsync();
    }

    public virtual async Task<E?> GetByIdAsync(int id)
    {
        return await _context.Set<E>().AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
    }

    public virtual async Task<E?> UpdateAsync(int id, E entity)
    {
        var updatedEntity = await _context.Set<E>().FindAsync(id);
        if (updatedEntity == null)
        {
            return null;
        }
        var entry = _context.Entry(updatedEntity);

        foreach (var property in entry.Properties)
        {
            if (property.Metadata.IsPrimaryKey())
            {
                continue;
            }

            property.CurrentValue = property.Metadata.PropertyInfo?.GetValue(entity);
        }
        return updatedEntity;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}