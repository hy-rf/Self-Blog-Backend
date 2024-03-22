using BBS.Data;
using BBS.Interfaces;
using BBS.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

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
            var roomlist = ctx.ChatRoom;
            var memberlist = ctx.ChatRoomMember.Where(crm => crm.UserId == UserId);
            var ret = from rl in roomlist
                      join ml in memberlist on rl.Id equals ml.ChatRoomId
                      select new ChatRoom
                      {
                          Id = rl.Id,
                          Name = rl.Name
                      };
            return ret.ToList();
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
        public List<ChatRoomMessage> GetChatMessages(int ChatRoomId)
        {
            var messages = ctx.ChatRoomMessage.Where(crm => crm.ChatRoomId == ChatRoomId).Include(crm => crm.User).Include(crm => crm.ChatRoom).ThenInclude(cr => cr.ChatRoomMembers).ThenInclude(crm => crm.User).ToList();
            return messages;
        }

        public List<ChatRoom> GetChatRooms(int UserId)
        {
            var rooms = ctx.ChatRoom.Include(cr => cr.ChatRoomMembers.Where(crm => crm.UserId == UserId)).ToList();
            return rooms;
        }
    }
}
