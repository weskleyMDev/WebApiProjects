using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using CatalogoAPI.DTOs;
using CatalogoAPI.Models;
using CatalogoAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CatalogoAPI.Controllers;

[Route("[controller]")]
[ApiController]
public class AuthController(ITokenService tokenService, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, ILogger<AuthController> logger) : ControllerBase
{
    private readonly ITokenService _tokenService = tokenService;
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;
    private readonly IConfiguration _configuration = configuration;
    private readonly ILogger<AuthController> _logger = logger;

    [HttpPost]
    [Route("create-role")]
    public async Task<IActionResult> CreateRole(string roleName)
    {
        var roleExists = await _roleManager.RoleExistsAsync(roleName);
        if (!roleExists)
        {
            var roleResult = await _roleManager.CreateAsync(new IdentityRole(roleName));
            if (roleResult.Succeeded)
            {
                _logger.LogInformation(1, "Role created successfully.");
                return Ok(new ResponseDTO { Status = "Success", Message = $"Role '{roleName}' created successfully!" });
            }
            else
            {
                _logger.LogInformation(2, "Role creation failed.");
                return BadRequest(new ResponseDTO { Status = "Error", Message = $"Failed to create role '{roleName}'." });
            }
        }
        return BadRequest(new ResponseDTO { Status = "Error", Message = $"Role '{roleName}' already exists!" });
    }

    [HttpPost]
    [Route("assign-role")]
    public async Task<IActionResult> AssignRole(string email, string roleName)
    {
        var user = await _userManager.FindByEmailAsync(email.ToLower());
        if (user != null)
        {
            var result = await _userManager.AddToRoleAsync(user, roleName.ToLower());
            if (result.Succeeded)
            {
                _logger.LogInformation(3, "Role assigned successfully.");
                return Ok(new ResponseDTO
                {
                    Status = "Success",
                    Message = $"Role '{roleName}' assigned to user '{email}' successfully!"
                });
            }
            else
            {
                _logger.LogInformation(4, "Role assignment failed.");
                return BadRequest(new ResponseDTO
                {
                    Status = "Error",
                    Message = $"Failed to assign role '{roleName}' to user '{email}'."
                });
            }
        }
        return NotFound(new ResponseDTO
        {
            Status = "Error",
            Message = $"User with email '{email}' not found!"
        });
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO model)
    {
        var user = await _userManager.FindByNameAsync(model.UserName!);
        if (user != null && await _userManager.CheckPasswordAsync(user, model.Password!))
        {
            var roles = await _userManager.GetRolesAsync(user);
            var authClaims = new List<Claim>
            {
                new(ClaimTypes.Name, user.UserName!),
                new(ClaimTypes.Email, user.Email!),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            foreach (var userRole in roles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }
            var token = _tokenService.CreateAccessToken(authClaims, _configuration);
            var refreshToken = _tokenService.CreateRefreshToken();
            _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInMinutes"], out int refreshTokenValidityInMinutes);
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(refreshTokenValidityInMinutes);
            user.RefreshToken = refreshToken;
            await _userManager.UpdateAsync(user);
            return Ok(new
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken,
                Expiration = token.ValidTo
            });
        }
        return Unauthorized();
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDTO model)
    {
        var userExists = await _userManager.FindByNameAsync(model.UserName!);
        if (userExists != null)
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDTO { Status = "Error", Message = "User already exists!" });

        var user = new ApplicationUser
        {
            UserName = model.UserName,
            Email = model.Email,
            SecurityStamp = Guid.NewGuid().ToString()
        };

        var result = await _userManager.CreateAsync(user, model.Password!);
        if (!result.Succeeded)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDTO { Status = "Error", Message = "User creation failed! Please check user details and try again." });
        }
        return Ok(new ResponseDTO { Status = "Success", Message = "User created successfully!" });
    }

    [HttpPost]
    [Route("refresh-token")]
    public async Task<IActionResult> RefreshToken(TokenDTO tokenDTO)
    {
        if (tokenDTO is null)
            return StatusCode(StatusCodes.Status400BadRequest, new ResponseDTO { Status = "Error", Message = "Invalid client request" });

        string? accessToken = tokenDTO.AccessToken ?? throw new ArgumentNullException(nameof(tokenDTO));
        string? refreshToken = tokenDTO.RefreshToken ?? throw new ArgumentNullException(nameof(tokenDTO));
        var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken!, _configuration);

        if (principal is null)
            return StatusCode(StatusCodes.Status400BadRequest, new ResponseDTO { Status = "Error", Message = "[principal]Invalid access token or refresh token" });

        string? username = principal.Identity?.Name;
        var user = await _userManager.FindByNameAsync(username!);

        if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            return StatusCode(StatusCodes.Status400BadRequest, new ResponseDTO { Status = "Error", Message = "[user]Invalid access token or refresh token" });

        var newAccessToken = _tokenService.CreateAccessToken([.. principal.Claims], _configuration);
        var newRefreshToken = _tokenService.CreateRefreshToken();
        user.RefreshToken = newRefreshToken;
        await _userManager.UpdateAsync(user);
        return new ObjectResult(new
        {
            Token = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
            RefreshToken = newRefreshToken
        });
    }

    [Authorize]
    [HttpPost]
    [Route("revoke/{username}")]
    public async Task<IActionResult> Revoke(string username)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user is null)
            return NotFound($"User {username} not found!");

        user.RefreshToken = null;
        await _userManager.UpdateAsync(user);
        return NoContent();
    }
}