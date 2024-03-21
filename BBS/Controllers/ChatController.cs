using BBS.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BBS.Controllers
{
    public class ChatController(IChatService chatService) : Controller
    {
        [Authorize]
        [Route("Chat/{Id}")]
        public IActionResult Index(int Id)
        {
            if (chatService.isInChatRoom(new Models.ChatRoomMember
            {
                Id = -1,
                UserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Sid)?.Value),
                ChatRoomId = Id,
            }))
            {
                ViewBag.ChatRoomId = Id;
                ViewBag.Id = Convert.ToInt32(User.FindFirst(ClaimTypes.Sid)?.Value);
                ViewBag.Name = User.FindFirst(ClaimTypes.Name)?.Value!.ToString();
                return View();
            }
            return Redirect("/");
        }
        public void CreateChatRoom()
        {
            throw new NotImplementedException();
        }
    }
}
