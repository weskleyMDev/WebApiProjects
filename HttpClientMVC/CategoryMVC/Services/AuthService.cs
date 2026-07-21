using System.Text;
using System.Text.Json;
using CategoryMVC.Models;

namespace CategoryMVC.Services;

public class AuthService : IAuthService
{
    private readonly IHttpClientFactory _httpFactory;
    private readonly JsonSerializerOptions _options;
    private TokenViewModel? _token;
    const string apiAuthEndpoint = "/Auth/login";

    public AuthService(IHttpClientFactory httpFactory)
    {
        _httpFactory = httpFactory;
        _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    public async Task<TokenViewModel?> AuthUser(UserViewModel userVM)
    {
        var client = _httpFactory.CreateClient("AuthApi");
        var user = JsonSerializer.Serialize(userVM);
        StringContent content = new(user, Encoding.UTF8, "application/json");

        using (var response = await client.PostAsync(apiAuthEndpoint, content))
        {
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStreamAsync();
                _token = await JsonSerializer.DeserializeAsync<TokenViewModel>(apiResponse, _options);
            } else
            {
                return null;
            }
        }

        return _token;
    }
}