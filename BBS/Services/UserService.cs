using BBS.Interfaces;
using BBS.Models;
using Microsoft.Data.Sqlite;
using System.Data.SqlTypes;

namespace BBS.Services
{
    public class UserService : IUserService
    {
        private readonly SqliteConnection Connection;
        public int UserId { get; set; }
        public UserService(IDatabase database)
        {
            Connection = database.SqLiteConnection();
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
                Connection.Open();
                var command = Connection.CreateCommand();
                command.Connection = Connection;
                command.CommandText = @"INSERT INTO User (Name, Password, LastLogin) VALUES ($Name, $Password, $LastLogin)";
                command.Parameters.AddWithValue("$Name", username);
                command.Parameters.AddWithValue("$Password", password);
                command.Parameters.AddWithValue("$LastLogin", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
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
            Connection.Open();
            var command = Connection.CreateCommand();
            command.Connection = Connection;
            command.CommandText = @"SELECT Id, Name FROM User WHERE Name = $username AND Password = $password";
            command.Parameters.AddWithValue("$username", username);
            command.Parameters.AddWithValue("$password", password);
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
            Connection.Open();
            var command = Connection.CreateCommand();
            command.Connection = Connection;
            command.CommandText = @"SELECT Id, Name, Password, Email, CreatedDate, LastLogin FROM User WHERE Id = $Id";
            command.Parameters.AddWithValue("$Id", Id);
            using SqliteDataReader reader = command.ExecuteReader();
            User user = new User();
            if (reader.Read())
            {
                user = new User
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Password = reader.GetString(2),
                    CreatedDate = reader.GetDateTime(4),
                    LastLogin = reader.GetDateTime(5)
                };
                Connection.Close();
            }
            return user;
        }
    }
}
