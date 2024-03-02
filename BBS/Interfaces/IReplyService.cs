using BBS.Models;

namespace BBS.Interfaces
{
    public interface IReplyService
    {
        public bool Reply(string Content, int UserId, string UserName, int PostId);
        public Reply GetReply(int PostId);
    }
}
