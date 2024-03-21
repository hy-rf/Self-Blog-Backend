using BBS.Data;
using BBS.Interfaces;
using BBS.Models;
using Microsoft.EntityFrameworkCore;

namespace BBS.Services
{
    public class ChatService(AppDbContext ctx) : IChatService
    {
        public bool isInChatRoom(ChatRoomMember chatRoomMember)
        {
            if (ctx.ChatRoomMember.Any(crm => crm.UserId == chatRoomMember.UserId && crm.ChatRoomId == chatRoomMember.ChatRoomId))
            {
                return true;
            }
            return false;
        }
        public void CreateChatMessage(ChatRoomMessage message)
        {
            ctx.ChatRoomMessage.Add(message);
            ctx.SaveChanges();
        }

        public void CreateChatRoom(ChatRoom chatRoom)
        {
            ctx.ChatRoom.Add(chatRoom);
            ctx.SaveChanges();
        }

        public List<ChatRoom> GetJoinedChatRooms(int UserId)
        {
            var roomlist = ctx.ChatRoom.Include(cr => cr.ChatRoomMembers.Where(crm => crm.UserId == UserId)).ToList();
            return roomlist;
        }

        public void AddMember(ChatRoomMember chatRoomMember)
        {
            ctx.ChatRoomMember.Add(chatRoomMember);
            ctx.SaveChanges();
        }

        public void KickMember(ChatRoomMember chatRoomMember)
        {
            ctx.ChatRoomMember.Remove(chatRoomMember);
            ctx.SaveChanges();
        }
    }
}
