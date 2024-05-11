﻿using BBS.IService;
using BBS.Models;
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
            System.Diagnostics.Debug.WriteLine(Context.User!.FindFirst(ClaimTypes.Sid)?.Value);
            if (string.IsNullOrEmpty(Message))
            {
                return;
            }
            DateTime Time = DateTime.Now;
            chatService.CreateChatMessage(new ChatRoomMessage
            {
                UserId = Convert.ToInt32(Context.User!.FindFirst(ClaimTypes.Sid)?.Value),
                Message = Message,
                Created = Time,
                ChatRoomId = Convert.ToInt32(RoomId),
            });
            await Clients.Group(RoomId).SendAsync("ReceiveMessage", RoomId, Convert.ToInt32(Context.User!.FindFirst(ClaimTypes.Sid)?.Value), Context.User!.FindFirst(ClaimTypes.Name)?.Value, Message, Time.ToLocalTime());
        }
    }
}
