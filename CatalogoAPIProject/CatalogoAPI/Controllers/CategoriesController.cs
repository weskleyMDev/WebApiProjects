using CatalogoAPI.DTOs;
using CatalogoAPI.DTOs.Mappings;
using CatalogoAPI.Filter;
using CatalogoAPI.Models;
using CatalogoAPI.Pagination;
using CatalogoAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Newtonsoft.Json;
using X.PagedList;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

namespace CatalogoAPI.Controllers;

[EnableCors("_originsAllowedAccess")]
// [EnableRateLimiting("_fixed")]
[Route("[controller]")]
[ApiController]
// ignore this controller in Swagger UI
// [ApiExplorerSettings(IgnoreApi = true)]
[Produces("application/json")]
public class CategoriesController(IUnitOfWork unitOfWork, IConfiguration configuration, ILogger<CategoriesController> logger, IMemoryCache memoryCache) : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IConfiguration _configuration = configuration;
    private readonly ILogger<CategoriesController> _logger = logger;
    private readonly IMemoryCache _memoryCache = memoryCache;
    private const string CacheKey = "CategoriesCache";

    [HttpGet("config")]
    public ActionResult<string> GetConfigValue()
    {
        var value = _configuration["key1"];
        var subkey2 = _configuration["section1:subkey2"];
        return $"{value} - {subkey2}" ?? "Key not found";
    }

    [HttpGet("cache")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetCategoriesFromCache()
    {
        if (!_memoryCache.TryGetValue(CacheKey, out IEnumerable<CategoryDTO>? categoriesDTO))
        {
            var categories = await _unitOfWork.CategoryRepository.GetAllAsync();

            if (categories is not null && categories.Any())
            {
                categoriesDTO = categories.ToDTOs();

                SetCategoryCache(CacheKey, categoriesDTO); // Set the cache for the list of categories
            }
            else
            {
                _logger.LogWarning("No categories found to cache.");
                return NotFound("No categories found!");
            }
        }

        return Ok(categoriesDTO);
    }

    // [Authorize]
    /// <summary>
    /// Get all categories
    /// </summary>
    /// <returns>A list of categories</returns>
    [HttpGet]
    [ServiceFilter(typeof(ApiLoggingFilter))]
    [DisableRateLimiting]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<IEnumerable<CategoryDTO>>> Get()
    {
        var categories = await _unitOfWork.CategoryRepository.GetAllAsync();
        if (categories is null)
        {
            return NotFound("No categories found!");
        }

        var categoriesDTO = categories.ToDTOs();

        return Ok(categoriesDTO);
    }

    [HttpGet("paginated")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetPaginated([FromQuery] CategoriesParameters categoriesParameters)
    {
        var categories = await _unitOfWork.CategoryRepository.GetCategoriesAsync(categoriesParameters);
        if (categories is null)
        {
            return NotFound("No categories found!");
        }

        return GetCategoriesDTO(categories);
    }

    [HttpGet("filter/name/paginated")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetCategoriesByName([FromQuery] CategoriesFilterName categoriesFilterName)
    {
        var categories = await _unitOfWork.CategoryRepository.GetCategoriesByNameAsync(categoriesFilterName);
        if (categories is null)
        {
            return NotFound("No categories found!");
        }

        return GetCategoriesDTO(categories);
    }

    private ActionResult<IEnumerable<CategoryDTO>> GetCategoriesDTO(IPagedList<Category> categories)
    {
        var metadata = new
        {
            categories.Count,
            categories.PageSize,
            categories.PageCount,
            categories.TotalItemCount,
            categories.HasNextPage,
            categories.HasPreviousPage
        };
        Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));
        var categoriesDTO = categories.ToDTOs();
        return Ok(categoriesDTO);
    }

    /// <summary>
    /// Get a category by its ID
    /// </summary>
    /// <param name="id">Category ID</param>
    /// <returns>A category with the specified ID</returns>
    [DisableCors]
    [HttpGet("{id:int:min(1)}", Name = "GetCategoryById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CategoryDTO>> Get(int id)
    {
        var CacheKeyById = GetCacheKeyForCategory(id);
        if (!_memoryCache.TryGetValue(CacheKeyById, out CategoryDTO? categoryDTO))
        {
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(c => c.CategoryId == id);
            if (category is null)
            {
                return NotFound($"Category {id} not found!");
            }
            categoryDTO = category.ToDTO();

            SetCategoryCache(CacheKeyById, categoryDTO); // Set the cache for the retrieved category
        }

        return Ok(categoryDTO);
    }


    /// <summary>
    /// Create a new category
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// {
    ///     POST api/categories
    ///     {
    ///       "categoryId": 0,
    ///       "name": "New Category",
    ///       "imageUrl": "https://example.com/new-category.jpg"
    ///     }
    /// }
    /// </remarks>
    /// <param name="categoryDTO">Category data</param>
    /// <returns>A new category created</returns>
    [DisableCors]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CategoryDTO>> Post(CategoryDTO categoryDTO)
    {
        if (categoryDTO is null)
        {
            return BadRequest("Invalid category data.");
        }
        var category = categoryDTO.ToEntity();

        var newCategory = _unitOfWork.CategoryRepository.Add(category!);
        await _unitOfWork.CommitAsync();

        var newCategoryDTO = newCategory.ToDTO();

        _memoryCache.Remove(CacheKey); // Invalidate the cache for the list of categories

        var CacheKeyById = GetCacheKeyForCategory(newCategoryDTO!.CategoryId);

        SetCategoryCache(CacheKeyById, newCategoryDTO); // Set the cache for the newly created category

        return new CreatedAtRouteResult("GetCategoryById", new { id = newCategoryDTO!.CategoryId }, newCategoryDTO);
    }

    [DisableCors]
    [HttpPut("{id:int:min(1)}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<CategoryDTO>> Put(int id, CategoryDTO categoryDTO)
    {
        if (id != categoryDTO.CategoryId)
        {
            return BadRequest("ID mismatch or Invalid category data.");
        }

        var category = categoryDTO.ToEntity();

        var updatedCategory = _unitOfWork.CategoryRepository.Update(category!);
        await _unitOfWork.CommitAsync();

        var updatedCategoryDTO = updatedCategory.ToDTO();

        var CacheKeyById = GetCacheKeyForCategory(updatedCategoryDTO!.CategoryId);
        SetCategoryCache(CacheKeyById, updatedCategoryDTO); // Update the cache for the updated category

        _memoryCache.Remove(CacheKey); // Invalidate the cache for the list of categories

        return Ok(updatedCategoryDTO);
    }

    [DisableCors]
    [HttpDelete("{id:int:min(1)}")]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<CategoryDTO>> Delete(int id)
    {
        var category = await _unitOfWork.CategoryRepository.GetByIdAsync(c => c.CategoryId == id);
        if (category is null)
        {
            return NotFound($"Category {id} not found!");
        }
        var deletedCategory = _unitOfWork.CategoryRepository.Delete(category);
        await _unitOfWork.CommitAsync();

        var deletedCategoryDTO = deletedCategory.ToDTO();

        _memoryCache.Remove(CacheKey); // Invalidate the cache for the list of categories
        _memoryCache.Remove($"CacheCategory_{id}"); // Invalidate the cache for the deleted category
        return Ok(deletedCategoryDTO);
    }

    private static string GetCacheKeyForCategory(int id) => $"CacheCategory_{id}";

    private void SetCategoryCache<T>(string key, T data)
    {
        var cacheEntryOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
            SlidingExpiration = TimeSpan.FromMinutes(2),
            Priority = CacheItemPriority.Normal
        };
        _memoryCache.Set(key, data, cacheEntryOptions);
    }

}