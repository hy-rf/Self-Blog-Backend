namespace BBS.Models
{
    public class FriendRequest
    {
        public int Id { get; set; }
        public int SendUserId { get; set; }
        public int ReceiveUserId { get; set; }
    }
}
