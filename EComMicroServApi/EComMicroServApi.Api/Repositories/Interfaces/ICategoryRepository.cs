using EComMicroServApi.Api.Models;
using EComMicroServApi.Api.Models.DTOs;

namespace EComMicroServApi.Api.Repositories.Interfaces;

public interface ICategoryRepository : IRepository<InputCategoryDto, OutputCategoryDto, Category>
{
}