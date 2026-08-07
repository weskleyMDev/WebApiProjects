using CatalogoAPIMinimal.ApiEndpoints;
using CatalogoAPIMinimal.ExtensionServices;
using CatalogoAPIMinimal.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.AddApiSwagger()
       .AddDbConnection()
       .AddAuthJwt();

builder.Services.AddCors();

builder.Services.Configure<JwtOptions>(
    builder.Configuration.GetSection("Jwt")
);

var app = builder.Build();

// app.MapGet("/", () => "Starting this Minimal API!");

app.MapAuthEndpoint();

app.MapCategoryEndpoints();

app.MapProductEndpoints();

var environment = app.Environment;

app.UseExceptionHandling(environment)
   .UseSwaggerMiddleware()
   .UseAppCors();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.Run();