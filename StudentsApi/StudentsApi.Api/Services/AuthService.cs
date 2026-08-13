
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using StudentsApi.Api.Data;
using StudentsApi.Api.Models;

namespace StudentsApi.Api.Services;

/// <summary>
/// 
/// </summary>
/// <param name="signInManager"></param>
/// <param name="userManager"></param>
/// <param name="jwtOptions"></param>
public class AuthService(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IOptions<JwtOptions> jwtOptions,
    AppDbContext dbContext) : IAuthService
{
    private readonly SignInManager<IdentityUser> _signInManager = signInManager;
    private readonly UserManager<IdentityUser> _userManager = userManager;
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;
    private readonly AppDbContext _dbContext = dbContext;

    /// <inheritdoc/>
    public async Task<bool> RegisterUser(string username, string password)
    {
        var appUser = new IdentityUser
        {
            UserName = username,
        };

        var result = await _userManager.CreateAsync(appUser, password);

        if (result.Succeeded)
        {
            await _signInManager.SignInAsync(appUser, false);
        }

        return result.Succeeded;
    }

    /// <inheritdoc/>
    public async Task<TokenModel?> SignInUser(string username, string password)
    {
        var user = await _userManager.FindByNameAsync(username);

        if (user is null)
        {
            return null;
        }

        var validPassword = await _userManager.CheckPasswordAsync(user, password);

        if (!validPassword)
        {
            return null;
        }

        var token = GenerateToken(user);

        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            TokenHash = HashRefreshToken(token.RefreshToken),
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = token.RefreshTokenExpiration
        };

        _dbContext.RefreshTokens.Add(refreshToken);
        await _dbContext.SaveChangesAsync();

        return token;
    }

    private TokenModel GenerateToken(IdentityUser user)
    {
        var claims = new[]
        {
        new Claim(JwtRegisteredClaimNames.Sub, user.Id),
        new Claim(ClaimTypes.Name, user.UserName!),
        new Claim(
            JwtRegisteredClaimNames.Jti,
            Guid.NewGuid().ToString())
    };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_jwtOptions.Key));

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256);

        var accessTokenExpiration = DateTime.UtcNow.AddMinutes(
            _jwtOptions.ExpirationMinutes);

        var refreshTokenExpiration = DateTime.UtcNow.AddDays(_jwtOptions.ExpirationDays);

        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            expires: accessTokenExpiration,
            signingCredentials: credentials
        );

        return new TokenModel
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
            AccessTokenExpiration = accessTokenExpiration,
            RefreshToken = GenerateRefreshToken(),
            RefreshTokenExpiration = refreshTokenExpiration
        };
    }

    private static string GenerateRefreshToken()
    {
        var randomBytes = RandomNumberGenerator.GetBytes(64);

        return Convert.ToBase64String(randomBytes);
    }

    private static string HashRefreshToken(string refreshToken)
    {
        var bytes = SHA512.HashData(
            Encoding.UTF8.GetBytes(refreshToken));

        return Convert.ToHexString(bytes);
    }

    /// <inheritdoc/>
    public async Task SignOutUser()
    {
        await _signInManager.SignOutAsync();
    }
}