using BBS.Interfaces;
using BBS.Models;
using Microsoft.Data.Sqlite;

namespace BBS.Services
{
    public class ReplyService : IReplyService
    {
        private readonly SqliteConnection Connection;
        public ReplyService(IDatabase database)
        {
            Connection = database.SqLiteConnection();
        }
        public bool Reply(string Content, int UserId, string UserName, int PostId){
            SqliteCommand ReplyCommand = new SqliteCommand{
                CommandText = @"INSERT INTO Reply (Content, UserId, UserName, ModifiedDate, PostId) VALUES ($Content, $UserId, $UserName, $ModifiedDate, $PostId)",
                Connection = Connection
            };
            ReplyCommand.Parameters.AddWithValue("$Content", Content);
            ReplyCommand.Parameters.AddWithValue("$UserId", UserId);
            ReplyCommand.Parameters.AddWithValue("$UserName", UserName);
            ReplyCommand.Parameters.AddWithValue("$ModifiedDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            ReplyCommand.Parameters.AddWithValue("$PostId", PostId);
            Connection.Open();
            if (ReplyCommand.ExecuteNonQuery()!=-1){
                Connection.Close();
                return true;
            }
            Connection.Close();
            return false;
        }
        public Reply GetReply(int PostId){
            throw new NotImplementedException();
        }
    }
}
