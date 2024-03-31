namespace BBS.Models
{
    public class ChatRoomMessage
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? Message { get; set; }
        public DateTime Created { get; set; }
        public int ChatRoomId { get; set; }
        public ChatRoom? ChatRoom { get; set; }
        public User? User { get; set; }
    }
}
