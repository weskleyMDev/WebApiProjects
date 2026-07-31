using System.ComponentModel.DataAnnotations.Schema;

namespace TarefasAPI.Data;

[Table("tarefas")]
public record Tarefa(int Id, string Atividade, string Status)
{
    
}