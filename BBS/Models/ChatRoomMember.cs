namespace BBS.Models
{
    public class ChatRoomMember
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ChatRoomId {  get; set; }
        public ChatRoom? ChatRoom { get; set; }
        public User? User { get; set; }
    }
}
