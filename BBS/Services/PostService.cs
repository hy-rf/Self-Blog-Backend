using System.Data;
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
            SqliteCommand CreatePostCommand = new SqliteCommand
            {
                Connection = Connection,
                CommandText = string.IsNullOrEmpty(Tags) ? @"INSERT INTO Post (Title, Content, UserId, ModifiedDate) VALUES ($Title, $Content, $UserId, $ModifiedDate)" : @"INSERT INTO Post (Title, Content, UserId, ModifiedDate, Tags) VALUES ($Title, $Content, $UserId, $ModifiedDate, $Tags)"
            };
            CreatePostCommand.Parameters.AddWithValue("$Title", Title);
            CreatePostCommand.Parameters.AddWithValue("$Content", Content);
            CreatePostCommand.Parameters.AddWithValue("$UserId", UserId);
            CreatePostCommand.Parameters.AddWithValue("$ModifiedDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            if (!string.IsNullOrEmpty(Tags))
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
        public bool ModifyPost(int Id, string Title, string Content, string? Tags)
        {
            throw new NotImplementedException();
        }
        public Post GetPost(int id)
        {
            throw new NotImplementedException();
        }
        public List<Post> GetRecentPosts()
        {
            Connection.Open();
            SqliteCommand GetRecentPostsCommand = new SqliteCommand
            {
                Connection = Connection,
                CommandText = @"SELECT Id, Title, Content, UserId, CreatedDate, ModifiedDate, Featured, Visibility, Tags, Likes FROM Post"
            };
            using var reader = GetRecentPostsCommand.ExecuteReader();
            List<Post> Posts = new List<Post>();
            while (reader.Read())
            {
                Posts.Add(new Post
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Title = reader.GetString(1),
                    Content = reader.GetString(2),
                    UserId = reader.GetInt32(3),
                    CreatedDate = reader.GetDateTime(4),
                    ModifiedDate = reader.GetDateTime(5),
                    Featured = reader.GetBoolean(6),
                    Visibility = reader.GetBoolean(7),
                    Tags = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                    Likes = reader.GetInt32(9)
                });
            }
            Connection.Close();
            return Posts;
        }
        public List<Post> GetPostsByUserId(int Id)
        {
            throw new NotImplementedException();
        }
        public List<Post> GetPostById(int Id){
            throw new NotImplementedException();
        }
    }
}
