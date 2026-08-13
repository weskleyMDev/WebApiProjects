using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
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
/// <param name="jwtOptions"></param>
/// <param name="authService"></param>
[ApiController]
[Route("api/[controller]")]
public class AuthController(
    IOptions<JwtOptions> jwtOptions,
    IAuthService authService
) : ControllerBase
{
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;
    private readonly IAuthService _authService = authService;

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
}