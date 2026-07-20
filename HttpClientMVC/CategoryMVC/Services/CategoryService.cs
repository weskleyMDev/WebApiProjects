using System.Text;
using System.Text.Json;
using CategoryMVC.Models;

namespace CategoryMVC.Services;

public class CategoryService : ICategoryService
{
    private const string apiEndPoint = "/Categories/";
    private readonly JsonSerializerOptions _options;
    private readonly IHttpClientFactory _clientFactory;
    private CategoryViewModel? categoryVM;
    private IEnumerable<CategoryViewModel>? categoriesVM;

    public CategoryService(IHttpClientFactory clientFactory)
    {
        _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        _clientFactory = clientFactory;
    }

    public async Task<IEnumerable<CategoryViewModel>?> GetCategories()
    {
        var client = _clientFactory.CreateClient("CategoriesApi");
        using (var response = await client.GetAsync(apiEndPoint))
        {
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStreamAsync();
                categoriesVM = await JsonSerializer.DeserializeAsync<IEnumerable<CategoryViewModel>>(apiResponse, _options);
            }
            else
            {
                return null;
            }
        }
        return categoriesVM;
    }

    public async Task<CategoryViewModel?> GetCategoryById(int id)
    {
        var client = _clientFactory.CreateClient("CategoriesApi");
        using (var response = await client.GetAsync(apiEndPoint + id))
        {
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStreamAsync();
                categoryVM = await JsonSerializer.DeserializeAsync<CategoryViewModel>(apiResponse, _options);
            }
            else
            {
                return null;
            }
        }
        return categoryVM;
    }

    public async Task<CategoryViewModel?> CreateCategory(CategoryViewModel categoryView)
    {
        var client = _clientFactory.CreateClient("CategoriesApi");
        var category = JsonSerializer.Serialize(categoryView);
        StringContent content = new(category, Encoding.UTF8, "application/json");
        using (var response = await client.PostAsync(apiEndPoint, content))
        {
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStreamAsync();
                categoryVM = await JsonSerializer.DeserializeAsync<CategoryViewModel>(apiResponse, _options);
            }
            else
            {
                return null;
            }
        }
        return categoryVM;
    }

    public async Task<bool> RemoveCategory(int id)
    {
        var client = _clientFactory.CreateClient("CategoriesApi");
        using var response = await client.DeleteAsync(apiEndPoint + id);
        if (response.IsSuccessStatusCode)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public async Task<bool> UpdateCategory(int id, CategoryViewModel categoryView)
    {
        var client = _clientFactory.CreateClient("CategoriesApi");
        using var response = await client.PutAsJsonAsync(apiEndPoint + id, categoryView);
        if (response.IsSuccessStatusCode)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}