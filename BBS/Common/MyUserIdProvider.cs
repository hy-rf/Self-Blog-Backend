
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace BBS.Common
{
    public class MyUserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            if (connection != null)
            {
                if (connection.User != null)
                {
                    return connection.User.FindFirst(ClaimTypes.Sid)?.Value!;
                }
            }
            return string.Empty;
        }
    }
}