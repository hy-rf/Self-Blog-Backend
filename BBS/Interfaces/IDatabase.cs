using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

namespace BBS.Interfaces
{
    public interface IDatabase
    {
        public SqliteConnection SqLiteConnection();
        public bool Execute(SqliteCommand sqliteCommand);
        public object GetRow(SqliteCommand sqliteCommand, string type, object obj);
        public List<object> GetRows(SqliteCommand sqliteCommand, List<object> list);
    }
}
