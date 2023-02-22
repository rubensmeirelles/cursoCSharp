using TarefasApiDapper.Endpoints;
using TarefasApiDapper.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddPersistence();
var app = builder.Build();

app.MapTarefasEndPoints();

app.Run();
