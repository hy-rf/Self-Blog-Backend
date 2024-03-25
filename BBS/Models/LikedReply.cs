namespace BBS.Models
{
    public class LikedReply
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ReplyId { get; set; }
        public Reply Reply { get; set; }
    }
}
