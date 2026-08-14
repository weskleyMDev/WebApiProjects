using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using StudentsApi.Api.DTOs;
using StudentsApi.Api.Models;
using StudentsApi.Api.Services;

namespace StudentsApi.Api.Controllers;

/// <summary>
/// 
/// </summary>
/// <param name="authService"></param>
[ApiController]
[Route("api/[controller]")]
public class AuthController(
    IAuthService authService,
    UserManager<IdentityUser> userManager
) : ControllerBase
{
    private readonly IAuthService _authService = authService;

    private readonly UserManager<IdentityUser> _userManager = userManager;

    /* [HttpPost("RegisterUser")]
    public async Task<ActionResult<TokenModel>> RegisterUser(RegisterModel model)
    {
        if (model.Password != model.ConfirmPassword)
        {
            return BadRequest(new ResponseMessage
            (
                $"Mismatch passwords!"
            ));
        }

        var result = await _authService.RegisterUser(model.Username, model.Password);

        if (result)
        {
            return Ok(new ResponseMessage(
                "Successfully registered user!"
            ));
        }
        else
        {
            return BadRequest();
        }
    } */

    /// <summary>
    /// 
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost("SigninUser")]
    public async Task<ActionResult<TokenModel>> SigninUser(LoginModel model)
    {
        var token = await _authService.SignInUser(
            model.Username,
            model.Password
        );

        if (token is null)
        {
            return Unauthorized();
        }

        return Ok(token);
    }

    [HttpPost("refresh")]
    public async Task<ActionResult> Refresh(RefreshTokenRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.RefreshToken))
            return BadRequest();

        var result = await _authService.RefreshToken(
            request.RefreshToken);

        if (result.Status != RefreshTokenResultStatus.Success)
            return Unauthorized();

        return Ok(result.Token);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout(
    [FromBody] LogoutRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.RefreshToken))
            return BadRequest();

        var revoked = await _authService.RevokeRefreshTokenAsync(
            request.RefreshToken);

        if (!revoked)
            return Unauthorized();

        return NoContent();
    }
}