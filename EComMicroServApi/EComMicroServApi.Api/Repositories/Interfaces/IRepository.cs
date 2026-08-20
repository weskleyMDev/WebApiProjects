namespace EComMicroServApi.Api.Repositories.Interfaces;

public interface IRepository<E>
{
    Task<IEnumerable<E>> GetAllAsync();
    Task<E?> GetByIdAsync(int id);
    E Create(E entity);
    Task<E?> UpdateAsync(int id, E entity);
    Task<bool> DeleteAsync(int id);
    Task SaveChangesAsync();
}