
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace BBS.Common
{
    public class MyUserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            return connection.User.FindFirst(ClaimTypes.Sid)?.Value;
        }
    }
}