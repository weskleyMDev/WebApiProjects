using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CatalogoAPI.Services;

public interface ITokenService
{
    JwtSecurityToken CreateAccessToken(IEnumerable<Claim> claims, IConfiguration _config);
    string CreateRefreshToken();
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token, IConfiguration _config);
}