namespace CatalogoAPIMinimal.ExtensionServices;

public static class AppBuilderExtensions
{
    public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder builder, IWebHostEnvironment environment)
    {
        if (environment.IsDevelopment())
        {
            builder.UseDeveloperExceptionPage();
        }

        return builder;
    }

    public static IApplicationBuilder UseAppCors(this IApplicationBuilder builder)
    {
        builder.UseCors(options =>
        {
            options.AllowAnyOrigin();
            options.WithMethods("GET");
            options.AllowAnyHeader();
        });

        return builder;
    }

    public static IApplicationBuilder UseSwaggerMiddleware(this IApplicationBuilder builder)
    {
        builder.UseSwagger();
        builder.UseSwaggerUI();

        return builder;
    }
}