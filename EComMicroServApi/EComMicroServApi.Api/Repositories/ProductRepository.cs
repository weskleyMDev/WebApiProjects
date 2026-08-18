using EComMicroServApi.Api.Data;
using EComMicroServApi.Api.Models;
using EComMicroServApi.Api.Models.DTOs;
using EComMicroServApi.Api.Repositories.Interfaces;

namespace EComMicroServApi.Api.Repositories;

public class ProductRepository(AppDbContext context) : Repository<InputProductDto, OutputProductDto, Product>(context), IProductRepository
{
}