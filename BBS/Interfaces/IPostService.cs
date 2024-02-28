using BBS.Models;

namespace BBS.Interfaces
{
    public interface IPostService
    {
        public bool CreatePost(string Title, string Content, int UserId, string? Tags);
        public Post GetPost(int id);
        public IEnumerable<Post> GetRecentPosts();
        public IEnumerable<Post> GetMostReplyPosts();
        public IEnumerable<Post> GetPosts();
    }
}
