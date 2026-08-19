using EComMicroServApi.Api.Data;
using EComMicroServApi.Api.Models;
using EComMicroServApi.Api.Repositories.Interfaces;
using EComMicroServApi.Api.Services.Interfaces;
using Mapster;

namespace EComMicroServApi.Api.Services;

public class CrudService<I, O, E, R>(AppDbContext context, R repository) : ICrudService<I, O> where I : class where O : class where E : class, IEntity where R : IRepository<E>
{
    protected readonly AppDbContext _context = context;
    protected readonly R _repository = repository;

    public async Task<O> CreateAsync(I entityDto)
    {
        var entity = entityDto.Adapt<E>();
        _repository.Create(entity);
        await _context.SaveChangesAsync();
        return entity.Adapt<O>();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var isDeleted = await _repository.DeleteAsync(id);
        if (!isDeleted)
        {
            return false;
        }
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<O>> GetAllAsync()
    {
        var entities = await _repository.GetAllAsync();
        return entities.Adapt<IEnumerable<O>>();
    }

    public async Task<O?> GetByIdAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity is null)
        {
            return null;
        }
        return entity.Adapt<O>();
    }

    public async Task<O?> UpdateAsync(int id, I entityDto)
    {
        var entity = entityDto.Adapt<E>();
        var updatedEntity = await _repository.UpdateAsync(id, entity);
        if (updatedEntity is null)
        {
            return null;
        }
        await _context.SaveChangesAsync();
        return updatedEntity.Adapt<O>();
    }
}