using BBS.Interfaces;
using BBS.Models;
using Microsoft.Data.Sqlite;

namespace BBS.Services
{
    public class PostService : IPostService
    {
        private readonly SqliteConnection Connection;
        private readonly IDatabase _database;
        public PostService(IDatabase database)
        {
            Connection = database.SqLiteConnection();
            _database = database;
        }
        public bool CreatePost(string Title, string Content, int UserId)
        {
            SqliteCommand CreatePostCommand = new SqliteCommand
            {
                Connection = Connection,
                CommandText = @"INSERT INTO Post (Title, Content, UserId, Created, Modified) VALUES ($Title, $Content, $UserId, $Created, $Modified)"
            };
            CreatePostCommand.Parameters.AddWithValue("$Title", Title);
            CreatePostCommand.Parameters.AddWithValue("$Content", Content);
            CreatePostCommand.Parameters.AddWithValue("$UserId", UserId);
            CreatePostCommand.Parameters.AddWithValue("$Created", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            CreatePostCommand.Parameters.AddWithValue("$Modified", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            Connection.Open();
            try
            {
                CreatePostCommand.ExecuteNonQuery();
                Connection.Close();
                return true;
            }
            catch
            {
                Connection.Close();
                return false;
            }
        }
        public bool EditPost(int Id, string Title, string Content)
        {
            SqliteCommand EditPostCommand = new SqliteCommand
            {
                Connection = Connection,
                CommandText = @"UPDATE Post SET Title=$Title, Content=$Content, Modified = $Modified WHERE Id=$Id"
            };
            EditPostCommand.Parameters.AddWithValue("$Title", Title);
            EditPostCommand.Parameters.AddWithValue("$Content", Content);
            EditPostCommand.Parameters.AddWithValue("$Modified", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            EditPostCommand.Parameters.AddWithValue("$Id", Id);
            Connection.Open();
            if (EditPostCommand.ExecuteNonQuery() != -1)
            {
                Connection.Close();
                return true;
            }
            Connection.Close();
            return false;
        }
        public object GetPost(int Id)
        {
            SqliteCommand getPost = new SqliteCommand
            {
                Connection = Connection,
                CommandText = @"SELECT Id, Title, Content, UserId, Created, Modified FROM Post WHERE Id = $Id"
            };
            getPost.Parameters.AddWithValue("$Id", Id);
            Connection.Open();
            Post post = new Post();
            using SqliteDataReader reader = getPost.ExecuteReader();
            if (reader.Read())
            {
                post.Id = reader.GetInt32(0);
                post.Title = reader.GetString(1);
                post.Content = reader.GetString(2);
                post.UserId = reader.GetInt32(3);
                post.Created = reader.GetDateTime(4);
                post.Modified = reader.GetDateTime(5);
            }
            Connection.Close();
            return post;
        }
        public List<Post> GetPosts()
        {
            Connection.Open();
            SqliteCommand GetRecentPostsCommand = new SqliteCommand
            {
                Connection = Connection,
                CommandText = @"SELECT Id, Title, Content, UserId, Created, Modified FROM Post"
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
                    Created = reader.GetDateTime(4),
                    Modified = reader.GetDateTime(5),
                });
            }
            Connection.Close();
            return Posts;
        }
        public List<Post> GetPostsByUserId(int Id)
        {
            throw new NotImplementedException();
        }
    }
}
