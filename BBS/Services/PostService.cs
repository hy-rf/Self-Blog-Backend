using BBS.Interfaces;
using BBS.Models;
using Microsoft.Data.Sqlite;

namespace BBS.Services
{
    public class PostService : IPostService
    {
        private readonly SqliteConnection Connection;
        public PostService(IDatabase database)
        {
            Connection = database.SqLiteConnection();
        }
        public bool CreatePost(string Title, string Content, int UserId, string? Tags)
        {
            SqliteCommand CreatePostCommand = new SqliteCommand();
            CreatePostCommand.Connection = Connection;
            CreatePostCommand.CommandText = (string.IsNullOrEmpty(Tags)) ? @"INSERT INTO Post (Title, Content, UserId) VALUES ($Title, $Content, $UserId)" : @"INSERT INTO Post (Title, Content, UserId, Tags) VALUES ($Title, $Content, $UserId, $Tags)";
            CreatePostCommand.Parameters.AddWithValue("$Title", Title);
            CreatePostCommand.Parameters.AddWithValue("$Content", Content);
            CreatePostCommand.Parameters.AddWithValue("$UserId", UserId);
            if (string.IsNullOrEmpty(Tags))
            {
                CreatePostCommand.Parameters.AddWithValue("$Tags", Tags);
            }
            Connection.Open();
            try
            {
                CreatePostCommand.ExecuteNonQuery();
                return true;
            }
            catch
            {
                Connection.Close();
                return false;
            }
        }
        public Post GetPost(int id)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<Post> GetRecentPosts()
        {
            throw new NotImplementedException();
        }
        public IEnumerable<Post> GetMostReplyPosts()
        {
            throw new NotImplementedException();
        }
        public IEnumerable<Post> GetPosts()
        {
            throw new NotImplementedException();
        }
    }
}
