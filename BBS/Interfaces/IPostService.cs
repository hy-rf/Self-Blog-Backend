using BBS.Models;

namespace BBS.Interfaces
{
    public interface IPostService
    {
        public bool CreatePost(string Title, string Content, int UserId, string? Tags);
        public bool ModifyPost(int Id, string Title, string Content, string? Tags);
        public Post GetPost(int id);
        public List<Post> GetRecentPosts();
        public List<Post> GetPostsByUserId(int Id);
        public List<Post> GetPostById(int Id);
    }
}
