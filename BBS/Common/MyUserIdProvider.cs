
using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;

namespace BBS.Common{
    public class MyUserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            return connection.User.FindFirst(ClaimTypes.Sid)?.Value;
        }
    }
}