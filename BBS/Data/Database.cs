using BBS.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.Data.Sqlite;
using System.Data;
using System.Reflection;

namespace BBS.Data
{
    public class Database(IConfiguration configuration) : IDatabase
    {
        private readonly string ConnectionString = configuration.GetConnectionString("LocalDB");
        public SqliteConnection SqLiteConnection()
        {
            return new SqliteConnection(ConnectionString);
        }
        public SqliteCommand BuildCommand(string CommandText, params object[] Args)
        {
            throw new NotImplementedException();
        }
        public bool Execute(SqliteCommand sqliteCommand)
        {
            throw new NotImplementedException();
        }
        public object GetRow(SqliteCommand sqliteCommand, string type, object obj)
        {
            switch (type)
            {
                case "IsExist":
                    if (sqliteCommand.ExecuteReader().Read())
                    {
                        return true;
                    }
                    return false;
                case "GetOne":
                    var reader = sqliteCommand.ExecuteReader();
                    if (reader.Read())
                    {
                        foreach (PropertyInfo prop in obj.GetType().GetProperties())
                        {
                            var data = Convert.ChangeType(reader.GetValue(prop.Name), prop.PropertyType);
                            prop.SetValue(obj, data);
                        }
                    }
                    return obj;
                default: throw new NotImplementedException();
            }
        }
        public List<object> GetRows(SqliteCommand sqliteCommand, List<object> list)
        {
            Type objElementType = list.GetType();
            var reader = sqliteCommand.ExecuteReader();
            while (reader.Read())
            {
                foreach (PropertyInfo prop in objElementType.GetType().GetProperties())
                {
                    var data = Convert.ChangeType(reader.GetValue(prop.Name), prop.PropertyType);
                    prop.SetValue(objElementType, data);
                }

                list.Add(objElementType);
            }
            return list;

        }
    }
}
