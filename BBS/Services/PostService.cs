using BBS.Data;
using BBS.Interfaces;
using BBS.Models;
using Microsoft.Data.Sqlite;

namespace BBS.Services
{
    public class PostService : IPostService
    {
        private readonly AppDbContext _appDbContext;
        public PostService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
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
            _appDbContext.Post.Add(newPost);
            _appDbContext.SaveChanges();
            return true;
        }
        public bool EditPost(int Id, string Title, string Content)
        {
            var EditPost = _appDbContext.Post.Single(p => p.Id == Id);
            EditPost.Title = Title;
            EditPost.Content = Content;
            EditPost.Modified = DateTime.Now;
            _appDbContext.SaveChanges();
            return true;
        }
        public object GetPost(int Id)
        {
            var GetPost = _appDbContext.Post.Where(p => p.Id == Id).ToList();
            return GetPost[0];
        }
        public List<Post> GetPosts()
        {
            var GetPosts = _appDbContext.Post.ToList();
            return GetPosts;
        }
        public List<Post> GetPostsByUserId(int Id)
        {
            throw new NotImplementedException();
        }
    }
}
