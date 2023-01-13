using Microsoft.EntityFrameworkCore;
using toDoWeb.Models;

namespace toDoWeb.Context
{
    public class TarefaContext : DbContext
    {
        public TarefaContext(DbContextOptions<TarefaContext> options) : base(options)
        {

        }

        public DbSet<Tarefa> Tarefas {get; set;}
    }
}