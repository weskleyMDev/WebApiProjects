namespace EComMicroServApi.Api.Repositories.Interfaces;

public interface IUnitOfWork
{
    public IProductRepository Products { get; }
    public ICategoryRepository Categories { get; }

    Task<int> SaveChangesAsync();
}