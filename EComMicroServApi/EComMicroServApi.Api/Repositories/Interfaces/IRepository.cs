namespace EComMicroServApi.Api.Repositories.Interfaces;

public interface IRepository<I, O, E>
{
    Task<IEnumerable<O>> GetAllAsync();
    Task<O?> GetByIdAsync(int id);
    E Add(I entityDto);
    Task<O?> UpdateAsync(int id, I entityDto);
    Task<bool> DeleteAsync(int id);
}