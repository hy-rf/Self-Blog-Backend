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
        public bool EditPost(int Id, string Title, string Content, string? Tags)
        {
            SqliteCommand EditPostCommand = new SqliteCommand
            {
                Connection = Connection,
                CommandText = string.IsNullOrEmpty(Tags) ? @"UPDATE Post SET Title=$Title, Content=$Content, ModifiedDate=$ModifiedDate WHERE Id=$Id" : @"UPDATE Post SET Title=$Title, Content=$Content, ModifiedDate=$ModifiedDate, Tags=$Tags WHERE Id=$Id"
            };
            EditPostCommand.Parameters.AddWithValue("$Title", Title);
            EditPostCommand.Parameters.AddWithValue("$Content", Content);
            EditPostCommand.Parameters.AddWithValue("$ModifiedDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            EditPostCommand.Parameters.AddWithValue("$Id", Id);
            Connection.Open();
            if (!string.IsNullOrEmpty(Tags))
            {
                EditPostCommand.Parameters.AddWithValue("$Tags", Tags);
            }
            if (EditPostCommand.ExecuteNonQuery() != -1)
            {
                Connection.Close();
                return true;
            }
            Connection.Close();
            return false;
        }
        public Post GetPost(int Id)
        {
            SqliteCommand getPost = new SqliteCommand
            {
                Connection = Connection,
                CommandText = @"SELECT Id, Title, Content, UserId, CreatedDate, ModifiedDate, Featured, Visibility, Tags, Likes FROM Post WHERE Id = $Id"
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
                post.CreatedDate = reader.GetDateTime(4);
                post.ModifiedDate = reader.GetDateTime(5);
                post.Featured = reader.GetBoolean(6);
                post.Visibility = reader.GetBoolean(7);
                post.Tags = reader.IsDBNull(8) ? string.Empty : reader.GetString(8);
                post.Likes = reader.GetInt32(9);
            }
            return post;
        }
        public List<Post> GetPosts()
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
    }
}
