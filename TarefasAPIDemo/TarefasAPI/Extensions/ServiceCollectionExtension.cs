using MySqlConnector;
using static TarefasAPI.Data.TarefaContext;

namespace TarefasAPI.Extensions;

public static class ServiceCollectionExtension
{
    public static WebApplicationBuilder AddPersistence(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

        builder.Services.AddScoped<GetConnection> (sp => async () =>
        {
            var connection = new MySqlConnection(connectionString);
            await connection.OpenAsync();
            return connection;
        });

        return builder;
    }
}