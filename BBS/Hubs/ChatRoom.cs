using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace BBS.Hubs
{
    public class ChatRoom : Hub
    {
        [Authorize]
        public async Task sendMessage(string Name, string Message)
        {
            await Clients.All.SendAsync("ReceiveMessage", Name, Message);
        }
    }
}
