
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using StudentsApi.Api.Data;
using StudentsApi.Api.DTOs;
using StudentsApi.Api.Models;

namespace StudentsApi.Api.Services;

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

    public async Task<RefreshToken?> FindRefreshTokenAsync(string refreshToken)
    {
        var tokenHash = HashRefreshToken(refreshToken);

        return await _dbContext.RefreshTokens.FirstOrDefaultAsync(
            rt => rt.TokenHash == tokenHash
        );
    }

    public async Task<RefreshTokenResult> RefreshToken(string refreshToken)
    {
        var storedRefreshToken =
        await FindRefreshTokenAsync(refreshToken);

        if (storedRefreshToken is null)
        {
            return new RefreshTokenResult
            {
                Status = RefreshTokenResultStatus.InvalidToken
            };
        }

        if (storedRefreshToken.ExpiresAt <= DateTime.UtcNow)
        {
            return new RefreshTokenResult
            {
                Status = RefreshTokenResultStatus.ExpiredToken
            };
        }

        if (storedRefreshToken.RevokedAt.HasValue)
        {
            await RevokeTokenFamilyAsync(storedRefreshToken);
            return new RefreshTokenResult
            {
                Status = RefreshTokenResultStatus.RevokedToken
            };
        }

        var user = await _userManager.FindByIdAsync(
            storedRefreshToken.UserId);

        if (user is null)
        {
            return new RefreshTokenResult
            {
                Status = RefreshTokenResultStatus.UserNotFound
            };
        }

        if (await _userManager.IsLockedOutAsync(user))
        {
            return new RefreshTokenResult
            {
                Status = RefreshTokenResultStatus.RevokedToken
            };
        }

        var token = GenerateToken(user);

        var newRefreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            TokenHash = HashRefreshToken(token.RefreshToken),
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = token.RefreshTokenExpiration
        };

        await using var transaction =
        await _dbContext.Database.BeginTransactionAsync();

        try
        {
            var revoked = await TryRevokeRefreshTokenAsync(
                storedRefreshToken,
                newRefreshToken.Id);

            if (!revoked)
            {
                await transaction.RollbackAsync();

                return new RefreshTokenResult
                {
                    Status = RefreshTokenResultStatus.RevokedToken
                };
            }

            _dbContext.RefreshTokens.Add(newRefreshToken);

            await _dbContext.SaveChangesAsync();

            await transaction.CommitAsync();

            return new RefreshTokenResult
            {
                Status = RefreshTokenResultStatus.Success,
                Token = token
            };
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    private async Task<RefreshToken?> FindReplacedTokenAsync(
    Guid tokenId)
    {
        return await _dbContext.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Id == tokenId);
    }

    private async Task<List<RefreshToken>> GetTokenChainAsync(
    RefreshToken refreshToken)
    {
        var chain = new List<RefreshToken>
    {
        refreshToken
    };

        var currentToken = refreshToken;

        while (currentToken.ReplacedByTokenId.HasValue)
        {
            var nextToken = await FindReplacedTokenAsync(
                currentToken.ReplacedByTokenId.Value);

            if (nextToken is null)
            {
                break;
            }

            chain.Add(nextToken);
            currentToken = nextToken;
        }

        return chain;
    }

    private async Task RevokeTokenFamilyAsync(
    RefreshToken refreshToken)
    {
        var tokenChain = await GetTokenChainAsync(refreshToken);

        var revokedAt = DateTime.UtcNow;

        foreach (var token in tokenChain)
        {
            if (!token.RevokedAt.HasValue)
            {
                token.RevokedAt = revokedAt;
            }
        }

        await _dbContext.SaveChangesAsync();
    }

    public async Task<bool> RevokeRefreshTokenAsync(string refreshToken)
    {
        var storedRefreshToken =
            await FindRefreshTokenAsync(refreshToken);

        if (storedRefreshToken is null)
        {
            return false;
        }

        if (storedRefreshToken.RevokedAt.HasValue)
        {
            return false;
        }

        storedRefreshToken.RevokedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();

        return true;
    }

    private async Task<bool> TryRevokeRefreshTokenAsync(
    RefreshToken refreshToken,
    Guid replacementTokenId)
    {
        var revokedAt = DateTime.UtcNow;

        var affectedRows = await _dbContext.RefreshTokens
            .Where(rt =>
                rt.Id == refreshToken.Id &&
                rt.RevokedAt == null)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(
                    rt => rt.RevokedAt,
                    revokedAt)
                .SetProperty(
                    rt => rt.ReplacedByTokenId,
                    replacementTokenId));

        return affectedRows == 1;
    }

    public async Task SignOutUser()
    {
        await _signInManager.SignOutAsync();
    }
}