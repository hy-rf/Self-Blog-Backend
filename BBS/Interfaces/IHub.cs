using BBS.Models;
using System.Xml.Linq;

namespace BBS.Interfaces
{
    public interface IHub
    {
        Task SendAsync(string ide, string RoomId, string UserId, string Name, string Message);
    }
}
