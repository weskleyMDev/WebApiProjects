namespace CatalogoAPI.Repositories;

public interface IUnitOfWork
{
    public IProductRepository ProductRepository { get; }
    public ICategoryRepository CategoryRepository { get; }

    void Commit();
}