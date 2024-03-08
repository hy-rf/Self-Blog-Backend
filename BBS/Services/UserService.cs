using BBS.Interfaces;
using BBS.Models;
using Microsoft.Data.Sqlite;
using System.Data.SqlTypes;

namespace BBS.Services
{
    public class UserService : IUserService
    {
        private readonly SqliteConnection Connection;
        private readonly IDatabase _database;
        public int UserId { get; set; }
        public UserService(IDatabase database)
        {
            Connection = database.SqLiteConnection();
            _database = database;
        }
        public int GetUserId()
        {
            return this.UserId;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool Signup(string username, string password)
        {
            if (CheckSignup(username))
            {
                var command = Connection.CreateCommand();
                command.Connection = Connection;
                command.CommandText = @"INSERT INTO User (Name, Pwd, Created, LastLogin, Avatar) VALUES ($Name, $Password, $Created, $LastLogin, $Avatar)";
                command.Parameters.AddWithValue("$Name", username);
                command.Parameters.AddWithValue("$Password", password);
                command.Parameters.AddWithValue("$Created", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                command.Parameters.AddWithValue("$LastLogin", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                command.Parameters.AddWithValue("$Avatar", "");
                Connection.Open();
                try
                {
                    command.ExecuteNonQuery();
                    Connection.Close();
                    return true;
                }
                catch
                {
                    Connection.Close();
                    return false;
                }
            }
            return false;
        }
        /// <summary>
        /// check if username is being used
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool CheckSignup(string username)
        {
            Connection.Open();
            var command = Connection.CreateCommand();
            command.Connection = Connection;
            command.CommandText = @"SELECT Name FROM User WHERE Name = $username";
            command.Parameters.AddWithValue("$username", username);
            using (SqliteDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    Connection.Close();
                    return false;
                }
            }
            Connection.Close();
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool Login(string username, string password)
        {
            var command = Connection.CreateCommand();
            command.Connection = Connection;
            command.CommandText = @"SELECT Id, Name FROM User WHERE Name = $username AND Pwd = $password";
            command.Parameters.AddWithValue("$username", username);
            command.Parameters.AddWithValue("$password", password);
            Connection.Open();
            using (SqliteDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    this.UserId = reader.GetInt32(0);
                    var updateLastLogin = Connection.CreateCommand();
                    updateLastLogin.Connection = Connection;
                    updateLastLogin.CommandText = @"UPDATE User SET LastLogin = $LastLogin WHERE Id = $Id";
                    updateLastLogin.Parameters.AddWithValue("LastLogin", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    updateLastLogin.Parameters.AddWithValue("$Id", this.UserId);
                    updateLastLogin.ExecuteNonQuery();
                    Connection.Close();
                    return true;
                }
            }
            Connection.Close();
            return false;
        }
        /// <summary>
        /// Used in UserCenter Page
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public User GetUser(int Id)
        {
            var command = Connection.CreateCommand();
            command.Connection = Connection;
            command.CommandText = @"SELECT Id, Name, Created, LastLogin, Avatar FROM User WHERE Id = $Id";
            command.Parameters.AddWithValue("$Id", Id);
            User user = new User();
            Connection.Open();
            using SqliteDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                user = new User
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Created = reader.GetDateTime(2),
                    LastLogin = reader.GetDateTime(3),
                    Avatar = reader.GetString(4)
                };
            }
            Connection.Close();
            return user;
        }
        public bool EditAvatar(int Id, string Avatar)
        {
            SqliteCommand EditAvatar = new SqliteCommand
            {
                CommandText = @"UPDATE User SET Avatar = $Avatar WHERE Id = $Id",
                Connection = Connection
            };
            EditAvatar.Parameters.AddWithValue("$Avatar", Avatar);
            EditAvatar.Parameters.AddWithValue("$Id", Id);
            Connection.Open();
            if (EditAvatar.ExecuteNonQuery() != -1)
            {
                Connection.Close();
                return true;
            }
            Connection.Close();
            return false;
        }
        public bool EditName(int Id, string Name)
        {
            SqliteCommand EditName = new SqliteCommand
            {
                CommandText = @"UPDATE User SET Name = $Name WHERE Id = $Id",
                Connection = Connection
            };
            EditName.Parameters.AddWithValue("$Name", Name);
            EditName.Parameters.AddWithValue("$Id", Id);
            Connection.Open();
            if (EditName.ExecuteNonQuery() != -1)
            {
                Connection.Close();
                return true;
            }
            return false;
        }
    }
}
