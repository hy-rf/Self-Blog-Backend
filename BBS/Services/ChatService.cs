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
        public void AddMember(ChatRoomMember chatRoomMember)
        {
            ctx.ChatRoomMember.Add(chatRoomMember);
            ctx.SaveChanges();
        }
        public void KickMember(ChatRoomMember chatRoomMember)
        {
            var torm = ctx.ChatRoomMember.Where(crm => crm.UserId == chatRoomMember.UserId && crm.ChatRoomId == chatRoomMember.ChatRoomId).Single();
            ctx.ChatRoomMember.Remove(torm);
            ctx.SaveChanges();
        }
        // Security unsafe
        public List<ChatRoomMessage> GetChatMessages(int ChatRoomId)
        {
            var messages = ctx.ChatRoomMessage.Where(crm => crm.ChatRoomId == ChatRoomId).Include(crm => crm.User).Include(crm => crm.ChatRoom).ThenInclude(cr => cr.ChatRoomMembers).ThenInclude(crm => crm.User).ToList();
            return messages;
        }
        public List<ChatRoomMessage> GetChatMessagesSimple(int ChatRoomId)
        {
            var messages = ctx.ChatRoomMessage.Where(crm => crm.ChatRoomId == ChatRoomId).Include(crm => crm.User).ToList();
            return messages;
        }
        public List<ChatRoom> GetJoinedChatRooms(int UserId)
        {
            var joinedRooms = ctx.ChatRoomMember
                .Where(crm => crm.UserId == UserId)
                .Select(crm => crm.ChatRoom)
                .ToList();
            return joinedRooms;
        }
    }
}
