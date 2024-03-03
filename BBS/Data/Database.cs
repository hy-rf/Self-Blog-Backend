using BBS.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.Data.Sqlite;
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
        public object Execute(SqliteCommand sqliteCommand, string type, object obj)
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
                        int o = 0;
                        o = Convert.ToInt32(o);
                        foreach (PropertyInfo prop in obj.GetType().GetProperties())
                        {
                            var data = Convert.ChangeType(reader.GetValue(o), prop.PropertyType);
                            prop.SetValue(obj, reader.IsDBNull(o) ? null : data);
                            o++;
                        }

                    }
                    return obj;
                //case "GetList":
                //    var reader = sqliteCommand.ExecuteReader();
                //    while (reader.Read())
                //    {
                //        foreach (PropertyInfo prop in obj.GetType().GetProperties())
                //        {
                //            prop.SetValue(obj, prop.GetValue(reader, null));
                //        }
                //        return obj;
                //    }
                default: throw new NotImplementedException();

            }
        }
    }
}
