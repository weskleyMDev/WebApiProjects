using CatalogoAPIMinimal.Models;

namespace CatalogoAPIMinimal.Services;

public interface ITokenService
{
    string GenerateToken(string key, string issuer, string audience, AppUser user);
}