using BBS.IService;
using BBS.Models;
using BBS.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace BBS.Hubs
{
    public class ChatRoom(IChatService chatService) : Hub
    {
        [Authorize]
        public async Task Join(string RoomId)
        {
            if (chatService.isInChatRoom(new ChatRoomMember
            {
                UserId = Convert.ToInt32(Context.User!.FindFirst(ClaimTypes.Sid)?.Value),
                ChatRoomId = Convert.ToInt32(RoomId)
            }))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, RoomId);
            }

        }
        public async Task SendMessage(string RoomId, string Message)
        {
            if (string.IsNullOrEmpty(Message))
            {
                return;
            }
            DateTime Time = DateTime.Now;
            ChatRoomMessage chatRoomMessage = new ChatRoomMessage
            {
                UserId = Convert.ToInt32(Context.User!.FindFirst(ClaimTypes.Sid)?.Value),
                Message = Message,
                Created = Time,
                ChatRoomId = Convert.ToInt32(RoomId),
            };
            chatService.CreateChatMessage(chatRoomMessage);
            ChatMessageViewModel newMsg = new ChatMessageViewModel
            {
                Id = chatRoomMessage.Id,
                ChatRoomId = Convert.ToInt32(RoomId),
                UserId = chatRoomMessage.UserId,
                Message = Message,
                Created = Time,
                UserName = Context.User!.FindFirst(ClaimTypes.Name)?.Value,
                UserAvatar = ""
            };
            await Clients.Group(RoomId).SendAsync("ReceiveMessage", newMsg);
        }
    }
}
