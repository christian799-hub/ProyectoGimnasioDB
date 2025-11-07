
using System.Data;
using Gimnasio.Core.Enum;

namespace Gimnasio.Infrastructure.Data
{
    public interface IDapperContext
    {
        DatabaseProvider Provider { get; }

        Task<IEnumerable<T>> QueryAsync<T>(string sql,object? param = null, 
        CommandType commandType = CommandType.Text);

        Task<T?> QueryFirstOrDefaultAsync<T>(string sql, object? param = null,
        CommandType commandType = CommandType.Text); 

        Task<int> ExecuteAsync(string sql, object? param = null,
        CommandType commandType = CommandType.Text);

        Task<T> ExecuteScalarAsync<T>(string sql, object? param = null,
        CommandType commandType = CommandType.Text); // Se usa para registros de los que se quiere obtener el maximo id de una tabla

        void SetAmbientConnection(IDbConnection conn, IDbTransaction? tx); 

        void ClearAmbientConnection();
    }
}