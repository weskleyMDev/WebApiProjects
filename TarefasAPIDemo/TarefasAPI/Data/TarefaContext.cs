using System.Data;

namespace TarefasAPI.Data;

public class TarefaContext
{
    public delegate Task<IDbConnection> GetConnection();
}