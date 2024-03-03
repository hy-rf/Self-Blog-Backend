using BBS.Interfaces;
using Microsoft.Data.Sqlite;

namespace BBS.Data
{
    public class Database(IConfiguration configuration) : IDatabase
    {
        private readonly string ConnectionString = configuration.GetConnectionString("LocalDB");
        public SqliteConnection SqLiteConnection()
        {
            return new SqliteConnection(ConnectionString);
        }
    }
}
