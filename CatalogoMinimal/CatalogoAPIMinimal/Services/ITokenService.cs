using CatalogoAPIMinimal.Models;

namespace CatalogoAPIMinimal.Services;

public interface ITokenService
{
    string GenerateToken(AppUser user);
}