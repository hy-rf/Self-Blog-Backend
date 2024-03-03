using BBS.Models;

namespace BBS.Interfaces
{
    public interface IPostService
    {
        public bool CreatePost(string Title, string Content, int UserId, string? Tags);
        public bool EditPost(int Id, string Title, string Content, string? Tags);
        public object GetPost(int id);
        public List<Post> GetPosts();
        public List<Post> GetPostsByUserId(int Id);
    }
}
