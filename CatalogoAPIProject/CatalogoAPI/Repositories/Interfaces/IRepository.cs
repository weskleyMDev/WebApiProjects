using System.Linq.Expressions;

namespace CatalogoAPI.Repositories.Interfaces;

public interface IRepository<T>
{
    T Add(T entity);
    T Delete(T entity);
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(Expression<Func<T, bool>> predicate);
    T Update(T entity);
}