namespace BBS.Models
{
    public class ChatRoom
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public List<ChatRoomMessage> ChatMessages { get; set; }
        public List<ChatRoomMember> ChatRoomMembers { get; set; }
    }
}
