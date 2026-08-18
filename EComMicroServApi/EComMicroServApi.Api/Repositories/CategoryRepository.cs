using EComMicroServApi.Api.Data;
using EComMicroServApi.Api.Models;
using EComMicroServApi.Api.Models.DTOs;
using EComMicroServApi.Api.Repositories.Interfaces;

namespace EComMicroServApi.Api.Repositories;

public class CategoryRepository(AppDbContext context) : Repository<InputCategoryDto, OutputCategoryDto, Category>(context), ICategoryRepository
{
}