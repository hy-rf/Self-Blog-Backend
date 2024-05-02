using BBS.Data;
using BBS.IService;
using BBS.Models;

namespace BBS.Services
{
    public class ReplyService(ForumContext ctx) : IReplyService
    {
        public bool Reply(string Content, int UserId, int PostId)
        {
            var newReply = new Reply
            {
                Content = Content,
                UserId = UserId,
                PostId = PostId,
                Created = DateTime.Now,
                Modified = DateTime.Now,
            };
            ctx.Reply.Add(newReply);
            ctx.SaveChanges();
            return true;
        }
        public List<Reply> GetReplies(int PostId)
        {
            var Replies = ctx.Reply.Where(r => r.PostId == PostId).ToList();
            return Replies;
        }
        public bool EditReply(int Id, string Content)
        {
            throw new NotImplementedException();
        }
    }
}
