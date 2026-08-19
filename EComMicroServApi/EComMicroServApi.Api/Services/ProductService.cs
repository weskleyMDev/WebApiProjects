using System.Collections;
using EComMicroServApi.Api.Data;
using EComMicroServApi.Api.Models;
using EComMicroServApi.Api.Models.DTOs;
using EComMicroServApi.Api.Repositories.Interfaces;
using EComMicroServApi.Api.Services.Interfaces;
using Mapster;

namespace EComMicroServApi.Api.Services;

public class ProductService(AppDbContext context, IProductRepository repository) : CrudService<InputProductDto, OutputProductDto, Product, IProductRepository>(context, repository), IProductService
{
}