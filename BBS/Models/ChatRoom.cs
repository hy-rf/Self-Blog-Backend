namespace BBS.Models
{
    public class ChatRoom
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime Created { get; set; }
        public List<ChatMessage> ChatMessages { get; set; }
        public List<ChatRoomMember> ChatRoomMembers { get; set; }
    }
}
