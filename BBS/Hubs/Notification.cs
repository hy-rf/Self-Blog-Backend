using System.Security.Claims;
using BBS.Interfaces;
using BBS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace BBS.Hubs
{
    public class Notification : Hub
    {
        public override async Task OnConnectedAsync()
        {
            try
            {
                int UserId = Convert.ToInt32(Context.User.FindFirst(ClaimTypes.Sid).Value);
                string UserName = Context.User.FindFirst(ClaimTypes.Name).Value;

                await Groups.AddToGroupAsync(Context.ConnectionId, UserId.ToString());
                await Clients.All.SendAsync("Join", $"{UserId} {UserName}");
            }
            catch
            {
                await Clients.All.SendAsync("Join", "Anonymous joined");
            }
        }
        public async Task SendMessage()
        {
            try
            {
                string Url = Context.GetHttpContext().Request.Path;
                await Clients.All.SendAsync("ReceiveNotification", Url);
            }
            catch
            {
                await Clients.All.SendAsync("ReceiveNotification", "Anonymous");
            }

        }
    }
}
