using System.Data;
using System.Data.SqlClient;
using Gimnasio.Core.Enum;
using Gimnasio.Core.Interfaces;
using Microsoft.Extensions.Configuration;


namespace Gimnasio.Infrastructure.Repositories
{

    public class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly string _sqlServerConnectionString;
        private readonly string _mySqlConnectionString;
        private readonly IConfiguration _config;
        

        public DbConnectionFactory(IConfiguration config)
        {
            _config = config;
            _mySqlConnectionString = _config.GetConnectionString("DefaultConnection") ?? string .Empty;
             _sqlServerConnectionString = _config.GetConnectionString("SqlServerConnection") ?? string.Empty;

            var providerStr = _config.GetSection("DatabaseProvider").Value ?? "MySql";

            Provider = providerStr.Equals("MySql", StringComparison.OrdinalIgnoreCase) 
            ? DatabaseProvider.MySql
            : DatabaseProvider.SqlServer;

        }
        public DatabaseProvider Provider { get; }  
        public IDbConnection CreateConnection()
        {
            return Provider switch
            {
                DatabaseProvider.MySql => new MySqlConnector.MySqlConnection(_mySqlConnectionString),
                _ => new SqlConnection(_sqlServerConnectionString)
            };
        }
    }
}