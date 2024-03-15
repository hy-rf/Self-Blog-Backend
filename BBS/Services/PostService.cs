using BBS.Data;
using BBS.Interfaces;
using BBS.Models;
using Microsoft.Data.Sqlite;

namespace BBS.Services
{
    public class PostService(AppDbContext ctx) : IPostService
    {
        public bool CreatePost(string Title, string Content, int UserId)
        {
            var newPost = new Post
            {
                UserId = UserId,
                Title = Title,
                Content = Content,
                Created = DateTime.Now,
                Modified = DateTime.Now
            };
            ctx.Post.Add(newPost);
            ctx.SaveChanges();
            return true;
        }
        public bool EditPost(int Id, string Title, string Content)
        {
            var EditPost = ctx.Post.Single(p => p.Id == Id);
            EditPost.Title = Title;
            EditPost.Content = Content;
            EditPost.Modified = DateTime.Now;
            ctx.SaveChanges();
            return true;
        }
        public Post GetPost(int Id)
        {
            var GetPost = ctx.Post.Single(p => p.Id == Id);
            return GetPost;
        }
        public List<Post> GetPosts()
        {
            var GetPosts = ctx.Post.ToList();
            return GetPosts;
        }
    }
}
