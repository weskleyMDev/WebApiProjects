using EComMicroServApi.Api.Services.Interfaces;

namespace EComMicroServApi.Api.Repositories.Interfaces;

public interface IUnitOfWork
{
    public ICategoryService CategoryService { get; }
    public IProductService ProductService { get; }
}