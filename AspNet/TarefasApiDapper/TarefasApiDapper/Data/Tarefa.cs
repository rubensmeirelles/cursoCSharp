using System.ComponentModel.DataAnnotations.Schema;

namespace TarefasApiDapper.Data;

[Table("Tarefas")]
public record Tarefa(int Id, string Atividade, string Status);


