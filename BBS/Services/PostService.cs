using BBS.Data;
using BBS.Interfaces;
using BBS.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace BBS.Services
{
    public class PostService(AppDbContext ctx) : IPostService
    {
        public bool CreatePost(string Title, string Content, string Tag, int UserId)
        {
            var newPost = new Post
            {
                UserId = UserId,
                Title = Title,
                Content = Content,
                Created = DateTime.Now,
                Modified = DateTime.Now
            };
            Tag.Split("#").ToList().ForEach(t =>
            {
                if (t != "")
                {
                    var tag = new Tag { Name = t, PostId = ctx.Post.Count() + 1 };
                    ctx.Tag.Add(tag);
                    ctx.SaveChanges();
                }
            });
            ctx.Post.Add(newPost);
            ctx.SaveChanges();
            return true;
        }
        public bool EditPost(int Id, string Title, string Content, string Tag)
        {
            var EditPost = ctx.Post.Single(p => p.Id == Id);
            EditPost.Title = Title;
            EditPost.Content = Content;
            EditPost.Modified = DateTime.Now;
            var oldtags = ctx.Tag.Where(t => t.PostId == Id);
            var newtags = Tag.Split("#").ToList();
            foreach (var t in oldtags)
            {
                if (!newtags.Contains(t.Name))
                {
                    ctx.Tag.Remove(t);
                }
            }
            foreach (var t in newtags)
            {
                if (t != "")
                {
                    if (!(ctx.Tag.Any(tag => tag.Name == t) && ctx.Tag.Any(tag => tag.PostId == Id)))
                    {
                        var tag = new Tag { Name = t, PostId = Id };
                        ctx.Tag.Add(tag);
                        ctx.SaveChanges();
                    }
                }
            }
                
            ctx.SaveChanges();
            return true;
        }
        public Post GetPost(int Id)
        {
            var GetPost = ctx.Post.Include(p => p.Tags).Single(p => p.Id == Id);
            return GetPost;
        }
        public List<Post> GetPosts()
        {
            var GetPosts = ctx.Post.Include(p => p.User).ToList();
            return GetPosts;
        }
    }
}
