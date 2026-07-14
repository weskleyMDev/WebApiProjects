using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.RateLimiting;
using Asp.Versioning;
using CatalogoAPI.Context;
using CatalogoAPI.DTOs.Mappings;
using CatalogoAPI.Extensions;
using CatalogoAPI.Filter;
using CatalogoAPI.Logging;
using CatalogoAPI.Models;
using CatalogoAPI.RateLimitOptions;
using CatalogoAPI.Repositories;
using CatalogoAPI.Repositories.Interfaces;
using CatalogoAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(ApiExceptionFilter));
}).AddJsonOptions(options =>
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles
).AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    c =>
    {
        // c.SwaggerDoc("v1", new OpenApiInfo { Title = "CatalogoAPI", Version = "v1" });
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "CatalogoAPI",
            Description = "API de catálogo de produtos",
            TermsOfService = new Uri("https://example.com/terms"),
            Contact = new OpenApiContact
            {
                Name = "Wesley Klug",
                Email = "wesley.klug@example.com",
                Url = new Uri("https://example.com/contact")
            },
            License = new OpenApiLicense
            {
                Name = "Use under LICX",
                Url = new Uri("https://example.com/license")
            }
        }
        );
        var xmlFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFileName));
        c.AddSecurityDefinition(
            "Bearer",
            new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Bearer JWT token"
            }
        );
        c.AddSecurityRequirement(
            new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            }
        );
    }
);

var secretKey = builder.Configuration["JWT:SecretKey"] ?? throw new InvalidOperationException("JWT Secret Key not found in configuration.");

builder.Services.AddAuthentication(
    options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }
).AddJwtBearer(
    options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
            ValidAudience = builder.Configuration["JWT:ValidAudience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
            ClockSkew = TimeSpan.Zero
        };
    }
);

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("AdminOnly", policy => policy.RequireRole("admin"))
    .AddPolicy("SuperAdminOnly", policy =>
        policy.RequireRole("admin").RequireClaim("id", "test"))
    .AddPolicy("UserOnly", policy => policy.RequireRole("user"))
    .AddPolicy("ExclusiveOnly", policy =>
        policy.RequireAssertion(context =>
            context.User.HasClaim(claim =>
                claim.Type == "id" && claim.Value == "test") ||
                context.User.IsInRole("SuperAdmin")));

//var originsAllowedAccess = "_originsAllowedAccess";
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: "_originsAllowedAccess",
        policy =>
        {
            policy.WithOrigins("https://apirequest.io")
                .WithMethods("GET")
                .AllowAnyHeader();
        });
});

var myRateLimitingOptions = new RateLimitOptions();
builder.Configuration.GetSection(RateLimitOptions.MyRateLimiting).Bind(myRateLimitingOptions);

builder.Services.AddRateLimiter(options =>
{
    /* options.AddFixedWindowLimiter("_fixed", limiter =>
    {
        limiter.PermitLimit = 1;
        limiter.Window = TimeSpan.FromSeconds(5);
        limiter.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        limiter.QueueLimit = 2;
    }); */
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
    {
        return RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.User.Identity?.Name ?? httpContext.Connection.RemoteIpAddress?.ToString() ?? "anonymous",
            factory: partition => new FixedWindowRateLimiterOptions
            {
                // PermitLimit = 2,
                PermitLimit = myRateLimitingOptions.PermitLimit,
                Window = TimeSpan.FromSeconds(myRateLimitingOptions.Window),
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = myRateLimitingOptions.QueueLimit
            });
    });
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
});

builder.Services
    .AddIdentityCore<ApplicationUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new QueryStringApiVersionReader(),
        new UrlSegmentApiVersionReader()
    );
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

string mySqlConnection = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<AppDbContext>(options => options.UseMySql(mySqlConnection, ServerVersion.AutoDetect(mySqlConnection)));

builder.Services.AddScoped<ApiLoggingFilter>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddAutoMapper(typeof(ProductDTOMappingProfile));

builder.Logging.AddProvider(new CustomLoggerProvider(new CustomLoggerProviderConfiguration
{
    LogLevel = LogLevel.Information,
}));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    // app.UseSwaggerUI();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "CatalogoAPI");
    });
    app.ConfigureExceptionHandler();
}

app.UseHttpsRedirection();

app.UseRateLimiter();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
