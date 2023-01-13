using Dapper.Contrib.Extensions;
using TarefasApiDapper.Data;
using static TarefasApiDapper.Data.TarefaContext;

namespace TarefasApiDapper.Endpoints
{
    public static class TarefasEndpoints
    {
        public static void MapTarefasEndPoints(this WebApplication app)
        {
            app.MapGet("/", () => $"Bem vindo a API de tarefas - {DateTime.Now}");

            app.MapGet("/tarefas", async (GetConnection connectionGetter) =>
            {
                using var con = await connectionGetter();
                var tarefas = con.GetAll<Tarefa>().ToList();

                if (tarefas is null)
                    return Results.NotFound();
                return Results.Ok(tarefas);
            });

            app.MapGet("/tarefas/{id}", async (GetConnection connectionGetter, int id) =>
            {
                using var con = await connectionGetter();
                //var tarefas = con.Get<Tarefa>(id);

                //if (tarefas is null)
                //    return Results.NotFound();
                //return Results.Ok(tarefas);

                return con.Get<Tarefa>(id) is Tarefa tarefa ? Results.Ok(tarefa) : Results.NotFound();
            });

            app.MapPost("/tarefas", async (GetConnection connectionGetter, Tarefa Tarefa) =>
            {
                using var con = await connectionGetter();
                var id = con.Insert(Tarefa);
                return Results.Created($"/tarefas/{id}", Tarefa);
            });

            app.MapPut("/tarefas", async (GetConnection connectionGetter, Tarefa Tarefa) =>
            {
                using var con = await connectionGetter();
                var id = con.Update(Tarefa);
                return Results.Ok();
            });

            app.MapDelete("/tarefas/{id}", async (GetConnection connectionGetter, int id) =>
            {
                using var con = await connectionGetter();
                var deleted = con.Get<Tarefa>(id);

                if (deleted is null)
                    return Results.NotFound();

                con.Delete(deleted);
                return Results.Ok(deleted);
            });
        }
    }
}
