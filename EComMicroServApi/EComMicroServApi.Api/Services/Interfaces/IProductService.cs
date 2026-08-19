using EComMicroServApi.Api.Models.DTOs;

namespace EComMicroServApi.Api.Services.Interfaces;

public interface IProductService : ICrudService<InputProductDto, OutputProductDto>
{
}