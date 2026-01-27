using System.Linq.Expressions;

namespace CatalogoAPI.Repositories;

public interface IRepository<T>
{
    T Add(T entity);
    T Delete(T entity);
    IEnumerable<T> GetAll();
    T? GetById(Expression<Func<T, bool>> predicate);
    T Update(T entity);
}