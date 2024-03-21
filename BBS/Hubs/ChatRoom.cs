using System.Security.Claims;
using BBS.Interfaces;
using BBS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace BBS.Hubs
{
    public class ChatRoom(IChatService chatService) : Hub
    {
        [Authorize]
        public async Task sendMessage(int RoomId, int UserId, string Name, string Message)
        {
            chatService.CreateChatMessage(new ChatRoomMessage
            {
                UserId = UserId,
                Message = Message,
                Created = DateTime.Now,
                ChatRoomId = RoomId,
            });
            await Clients.All.SendAsync("ReceiveMessage", Name, Message);
        }
    }
}
