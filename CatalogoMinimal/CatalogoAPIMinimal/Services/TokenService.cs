using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CatalogoAPIMinimal.Models;
using Microsoft.IdentityModel.Tokens;

namespace CatalogoAPIMinimal.Services;

public class TokenService : ITokenService
{
    public string GenerateToken(string key, string issuer, string audience, AppUser user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Username!),
            new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())
        };

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(issuer: issuer, audience: audience, claims: claims, expires: DateTime.UtcNow.AddMinutes(120), signingCredentials: credentials);

        var tokenHandler = new JwtSecurityTokenHandler();
        var stringToken = tokenHandler.WriteToken(token);

        return stringToken;
    }
}