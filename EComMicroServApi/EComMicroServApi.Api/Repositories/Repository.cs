using EComMicroServApi.Api.Data;
using EComMicroServApi.Api.Models;
using EComMicroServApi.Api.Repositories.Interfaces;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace EComMicroServApi.Api.Repositories;

public class Repository<I, O, E>(AppDbContext context) : IRepository<I, O, E> where I : class where O : class where E : class, IEntity
{
    protected readonly AppDbContext _context = context;

    public O Add(I entityDto)
    {
        var entity = entityDto.Adapt<E>();
        _context.Set<E>().Add(entity);
        return entity.Adapt<O>();
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

    public async Task<IEnumerable<O>> GetAllAsync()
    {
        var entities = await _context.Set<E>().AsNoTracking().ToListAsync();
        return entities.Adapt<IEnumerable<O>>();
    }

    public async Task<O?> GetByIdAsync(int id)
    {
        var entity = await _context.Set<E>().AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
        if (entity == null)
        {
            return null;
        }
        return entity.Adapt<O>();
    }

    public async Task<O?> UpdateAsync(int id, I entityDto)
    {
        var entity = await _context.Set<E>().FindAsync(id);
        if (entity == null)
        {
            return null;
        }
        entityDto.Adapt(entity);
        return entity.Adapt<O>();
    }
}