using BBS.Interfaces;
using BBS.Models;
using Microsoft.Data.Sqlite;
using System.Data.SqlTypes;

namespace BBS.Services
{
    public class UserService : IUserService
    {
        private readonly SqliteConnection Connection;
        public UserService(IDatabase database)
        {
            Connection = database.SqLiteConnection();
        }
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
                command.Parameters.AddWithValue("$LastLogin", DateTime.Now);
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
            System.Diagnostics.Debug.WriteLine(username);
            var command = Connection.CreateCommand();
            command.Connection = Connection;
            command.CommandText = $"SELECT Name FROM User WHERE Name = @username";
            command.Parameters.AddWithValue("@username", username);
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
        public bool Login(string username, string password)
        {
            throw new NotImplementedException();
        }
        public bool VerifyPassword(string username)
        {
            throw new NotImplementedException();
        }
        public User GetUser(int Id)
        {
            throw new NotImplementedException();
        }

        Models.User IUserService.GetUser(int Id)
        {
            throw new NotImplementedException();
        }
    }
}
