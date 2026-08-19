namespace EComMicroServApi.Api.Services.Interfaces;

public interface ICrudService<I, O>
{
    Task<IEnumerable<O>> GetAllAsync();
    Task<O?> GetByIdAsync(int id);
    Task<O> CreateAsync(I entityDto);
    Task<O?> UpdateAsync(int id, I entityDto);
    Task<bool> DeleteAsync(int id);
}