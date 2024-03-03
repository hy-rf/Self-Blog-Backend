using BBS.Models;

namespace BBS.Interfaces
{
    public interface IReplyService
    {
        public bool Reply(string Content, int UserId, int PostId);
        public List<Reply> GetReplies(int PostId);
    }
}
