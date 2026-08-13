using Microsoft.AspNetCore.Identity;
using StudentsApi.Api.Models;

namespace StudentsApi.Api.Services;

public interface IAuthService
{
    Task<bool> RegisterUser(string username, string password);
    Task<TokenModel?> SignInUser(string username, string password);
    Task SignOutUser();
}