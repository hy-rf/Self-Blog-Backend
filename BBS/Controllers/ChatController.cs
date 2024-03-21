using BBS.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BBS.Controllers
{
    public class ChatController(IChatService chatService) : Controller
    {
        [Authorize]
        public IActionResult Index()
        {
            ViewBag.Id = Convert.ToInt32(User.FindFirst(ClaimTypes.Sid)?.Value);
            ViewBag.Name = User.FindFirst(ClaimTypes.Name)?.Value!.ToString();
            var ret = chatService.GetChatRooms(Convert.ToInt32(User.FindFirst(ClaimTypes.Sid)?.Value));
            return View(ret);
        }
        [Authorize]
        [Route("ChatRoom/{Id}")]
        public IActionResult ChatRoom(int Id)
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
                return View(chatService.GetChatMessages(ViewBag.Id, Id));
            }
            return Redirect("/");
        }
        [HttpPost]
        public void CreateChatRoom(string Name)
        {
            var newchatroom = new Models.ChatRoom
            {
                Name = Name
            };
            chatService.CreateChatRoom(newchatroom);
            chatService.AddMember(new Models.ChatRoomMember
            {
                ChatRoomId = newchatroom.Id,
                UserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Sid)?.Value)
            });
        }
    }
}
