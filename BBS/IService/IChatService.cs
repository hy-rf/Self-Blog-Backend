using BBS.Models;
using BBS.ViewModels;

namespace BBS.IService
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
        public List<ChatMessageViewModel> GetChatMessagesSimple(int ChatRoomId);
        public List<ChatRoomMember> GetChatRoomMembers(int ChatRoomId);
    }
}
