using BBS.Models;

namespace BBS.Interfaces
{
    public interface IChatService
    {
        public bool isInChatRoom(ChatRoomMember chatRoomMember);
        public List<ChatRoom> GetJoinedChatRooms(int UserId);
        public void CreateChatRoom(ChatRoom chatRoom);
        public void AddMember(ChatRoomMember chatRoomMember);
        public void CreateChatMessage(ChatRoomMessage message);
        public void KickMember(ChatRoomMember chatRoomMember);
        public List<ChatRoomMessage> GetChatMessages(int ChatRoomId);
        public List<ChatRoom> GetChatRooms(int UserId);
    }
}
