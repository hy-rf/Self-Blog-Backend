using BBS.Interfaces;
using Microsoft.Data.Sqlite;

namespace BBS.Data
{
    public class Database(IConfiguration configuration) : IDatabase
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly string ConnectionString = configuration.GetConnectionString("LocalDB");
        public SqliteConnection SqLiteConnection()
        {
            using SqliteConnection connection = new SqliteConnection(ConnectionString);
            return connection;
        }
    }
}
