using Microsoft.AspNetCore.Identity;
using StudentsApi.Api.DTOs;
using StudentsApi.Api.Models;

namespace StudentsApi.Api.Services;

public interface IAuthService
{
    Task<bool> RegisterUser(string username, string password);
    Task<TokenModel?> SignInUser(string username, string password);
    Task<RefreshToken?> FindRefreshTokenAsync(string refreshToken);
    Task<RefreshTokenResult> RefreshToken(string refreshToken);
    Task<bool> RevokeRefreshTokenAsync(string refreshToken);
    Task SignOutUser();
}