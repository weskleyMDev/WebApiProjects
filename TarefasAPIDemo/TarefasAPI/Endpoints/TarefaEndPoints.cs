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
                return Results.NotFound("Nenhuma tarefa encontrada!");
            }

            return Results.Ok(tarefas);
        });

        app.MapGet("/tarefas/{id}", async (int id, GetConnection connection) =>
        {
            using var con = await connection();
            var tarefa = con.Get<Tarefa>(id);

            if (tarefa is null)
            {
                return Results.NotFound($"Tarefa com id = {id} não encontrado!");
            }

            return Results.Ok(tarefa);
        });

        app.MapPost("/tarefas", async (Tarefa tarefa, GetConnection connection) =>
        {
            using var con = await connection();
            var id = con.Insert(tarefa);

            return Results.Created($"/tarefas/{id}", tarefa);
        });

        app.MapPut("/tarefas", async (Tarefa tarefa, GetConnection connection) =>
        {
            using var con = await connection();
            var result = con.Update(tarefa);

            if (result)
            {
                return Results.Ok();
            }

            return Results.NotFound($"Tarefa não encontrada!");
        });

        app.MapDelete("/tarefas/{id}", async (int id, GetConnection connection) =>
        {
            using var con = await connection();
            var tarefa = con.Get<Tarefa>(id);

            if (tarefa is null)
            {
                return Results.NotFound($"Tarefa com id = {id} não encontrado!");
            }

            con.Delete(tarefa);
            return Results.Ok(tarefa);
        });
    }
}