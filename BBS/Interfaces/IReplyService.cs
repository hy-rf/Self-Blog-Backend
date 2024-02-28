using BBS.Models;

namespace BBS.Interfaces
{
    public interface IReplyService
    {
        public Reply GetPost(int id);
        public IEnumerable<Reply> GetReplies(int PostId);
        public IEnumerable<Reply> GetPosts();
    }
}
