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
        public bool Reply(string Content, int UserId, int PostId){
            SqliteCommand ReplyCommand = new SqliteCommand{
                CommandText = @"INSERT INTO Reply (Content, UserId, PostId, Created, Modified) VALUES ($Content, $UserId, $PostId, $Created, $Modified)",
                Connection = Connection
            };
            ReplyCommand.Parameters.AddWithValue("$Content", Content);
            ReplyCommand.Parameters.AddWithValue("$UserId", UserId);
            ReplyCommand.Parameters.AddWithValue("$PostId", PostId);
            ReplyCommand.Parameters.AddWithValue("$Created", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            ReplyCommand.Parameters.AddWithValue("$Modified", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            Connection.Open();
            if (ReplyCommand.ExecuteNonQuery()!=-1){
                Connection.Close();
                return true;
            }
            Connection.Close();
            return false;
        }
        public List<Reply> GetReplies(int PostId){
            SqliteCommand RepliesCommand = new SqliteCommand
            {
                CommandText = @"SELECT Content, UserId, Created, Modified FROM Reply WHERE PostId = $PostId",
                Connection = Connection
            };
            RepliesCommand.Parameters.AddWithValue("$PostId", PostId);
            Connection.Open();
            List<Reply> Replies = new List<Reply>();
            using (var reader = RepliesCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    Replies.Add(new Reply
                    {
                        Content = reader.GetString(0),
                        UserId = reader.GetInt32(1),
                        Created = reader.GetDateTime(2),
                        Modified = reader.GetDateTime(3)
                    });
                }
            }
            return Replies;
        }
    }
}
