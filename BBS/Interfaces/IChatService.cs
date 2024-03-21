using BBS.Models;

namespace BBS.Interfaces
{
    public interface IChatService
    {
        public bool isInChatRoom(ChatRoomMember chatRoomMember);
        public List<ChatRoom> GetJoinedChatRooms(int UserId);
        public void CreateChatRoom(ChatRoom chatRoom);
        public void InviteMember();
        public void CreateChatMessage(ChatRoomMessage message);
        public void KickMember(ChatRoomMember chatRoomMember);
    }
}
