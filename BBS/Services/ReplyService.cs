using BBS.Data;
using BBS.Interfaces;
using BBS.Models;
using Microsoft.Data.Sqlite;

namespace BBS.Services
{
    public class ReplyService : IReplyService
    {
        private readonly AppDbContext _appDbContext;
        private readonly SqliteConnection Connection;
        public ReplyService(IDatabase database, AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
            Connection = database.SqLiteConnection();
        }
        public bool Reply(string Content, int UserId, int PostId)
        {
            var newReply = new Reply{
                Content = Content,
                UserId = UserId,
                PostId = PostId,
                Created = DateTime.Now,
                Modified = DateTime.Now,
            };
            _appDbContext.Reply.Add(newReply);
            _appDbContext.SaveChanges();
            return true;
        }
        public List<Reply> GetReplies(int PostId)
        {
            var Replies = _appDbContext.Reply.Where(r => r.PostId == PostId).ToList();
            return Replies;
        }
        public bool EditReply(int Id, string Content)
        {
            throw new NotImplementedException();
        }
    }
}
