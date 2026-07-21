using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using CategoryMVC.Models;

namespace CategoryMVC.Services;

public class ProductService : IProductService
{
    private readonly IHttpClientFactory _httpClient;
    private readonly JsonSerializerOptions _options;
    private ProductViewModel? productV;
    private IEnumerable<ProductViewModel>? productsVM;
    private const string apiEndpoint = "/Products/";

    public ProductService(IHttpClientFactory httpClient)
    {
        _httpClient = httpClient;
        _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    public async Task<ProductViewModel?> CreateProduct(ProductViewModel productVM, string token)
    {
        var client = _httpClient.CreateClient("ProductsApi");
        PutTokenInHeaderAuthorization(token, client);

        var product = JsonSerializer.Serialize(productVM);
        StringContent content = new(product, Encoding.UTF8, "application/json");

        using (var response = await client.PostAsync(apiEndpoint, content))
        {
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStreamAsync();
                productV = await JsonSerializer.DeserializeAsync<ProductViewModel>(apiResponse, _options);
            }
            else
            {
                return null;
            }
        }
        return productV;
    }

    public async Task<ProductViewModel?> GetProductById(int id, string token)
    {
        var client = _httpClient.CreateClient("ProductsApi");
        PutTokenInHeaderAuthorization(token, client);
        using (var response = await client.GetAsync(apiEndpoint + id))
        {
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStreamAsync();
                productV = await JsonSerializer.DeserializeAsync<ProductViewModel>(apiResponse, _options);
            }
            else
            {
                return null;
            }
        }
        return productV;
    }

    public async Task<IEnumerable<ProductViewModel>?> GetProducts(string token)
    {
        var client = _httpClient.CreateClient("ProductsApi");
        PutTokenInHeaderAuthorization(token, client);
        using (var response = await client.GetAsync(apiEndpoint))
        {
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStreamAsync();
                productsVM = await JsonSerializer.DeserializeAsync<IEnumerable<ProductViewModel>>(apiResponse, _options);
            }
            else
            {
                return null;
            }
        }
        return productsVM;
    }

    public async Task<bool> RemoveProduct(int id, string token)
    {
        var client = _httpClient.CreateClient("ProductsApi");
        PutTokenInHeaderAuthorization(token, client);

        using var response = await client.DeleteAsync(apiEndpoint + id);
        if (response.IsSuccessStatusCode)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public async Task<bool> UpdateProduct(int id, ProductViewModel productVM, string token)
    {
        var client = _httpClient.CreateClient("ProductsApi");
        PutTokenInHeaderAuthorization(token, client);

        using var response = await client.PutAsJsonAsync(apiEndpoint + id, productVM);
        if (response.IsSuccessStatusCode)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private static void PutTokenInHeaderAuthorization(string token, HttpClient client)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
}