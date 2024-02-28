using BBS.Interfaces;
using BBS.Models;

namespace BBS.Services
{
    public class ReplyService : IReplyService
    {
        public Reply GetPost(int id)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<Reply> GetReplies(int PostId)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<Reply> GetPosts()
        {
            throw new NotImplementedException();
        }
    }
}
