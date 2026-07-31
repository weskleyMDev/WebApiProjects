using Dapper.Contrib.Extensions;
using TarefasAPI.Data;
using static TarefasAPI.Data.TarefaContext;

namespace TarefasAPI.EndPoints;

public static class TarefaEndPoints
{
    public static void MapTarefasEndPoints(this WebApplication app)
    {
        app.MapGet("/", () => $"Welcome to Tarefas API - {DateTime.Now}");

        app.MapGet("/tarefas", async (GetConnection connection) =>
        {
            using var con = await connection();
            var tarefas = con.GetAll<Tarefa>().ToList();

            if (tarefas is null)
            {
                return Results.NotFound();
            }

            return Results.Ok(tarefas);
        });
    }
}