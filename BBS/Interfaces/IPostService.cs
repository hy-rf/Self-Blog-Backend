using BBS.Models;

namespace BBS.Interfaces
{
    public interface IPostService
    {
        public Post GetPost(int id);
        public IEnumerable<Post> GetRecentPosts();
        public IEnumerable<Post> GetMostReplyPosts();
        public IEnumerable<Post> GetPosts();
    }
}
