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
        public async Task SendMessage(string RoomId, string UserId, string Name, string Message)
        {
            chatService.CreateChatMessage(new ChatRoomMessage
            {
                UserId = Convert.ToInt32(Context.User!.FindFirst(ClaimTypes.Sid)?.Value),
                Message = Message,
                Created = DateTime.Now,
                ChatRoomId = Convert.ToInt32(RoomId),
            });
            await Clients.All.SendAsync("ReceiveMessage", RoomId, UserId, Name, Message);
        }
    }
}
