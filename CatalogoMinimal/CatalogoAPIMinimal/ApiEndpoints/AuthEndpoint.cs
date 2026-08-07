using CatalogoAPIMinimal.Models;
using CatalogoAPIMinimal.Services;
using Microsoft.AspNetCore.Authorization;

namespace CatalogoAPIMinimal.ApiEndpoints;

public static class AuthEndpoint
{
    public static void MapAuthEndpoint(this WebApplication app)
    {
        app.MapPost("/login", [AllowAnonymous] (AppUser user, ITokenService service) =>
{
    if (user is null)
    {
        return Results.BadRequest(new
        {
            message = "Invalid username/password!"
        });
    }

    if (user.Username == "test" && user.Password == "123456")
    {
        var tokenString = service.GenerateToken(app.Configuration["Jwt:Key"]!, app.Configuration["Jwt:Issuer"]!, app.Configuration["Jwt:Audience"]!, user);

        return Results.Ok(new { token = tokenString });
    }
    else
    {
        return Results.BadRequest(new { message = "Invalid Login!" });
    }
}).Produces(StatusCodes.Status200OK)
  .Produces(StatusCodes.Status400BadRequest)
  .WithName("AuthUser")
  .WithTags("Login");
    }
}