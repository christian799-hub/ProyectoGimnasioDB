
using System.Data;
using Gimnasio.Core.Enum;

namespace Gimnasio.Core.Interfaces
{
    public interface IDbConnectionFactory
    {
        DatabaseProvider Provider { get; }
        IDbConnection CreateConnection();
    }
}