﻿using BBS.Interfaces;
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
        public bool ModifyPost(int Id, string Title, string Content, string? Tags){
            throw new NotImplementedException();
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
