using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BBS.Controllers
{
    public class ChatController : Controller
    {
        [Authorize]
        [Route("Chat/{Id}")]
        public IActionResult Index(int Id)
        {
            ViewBag.ChatRoomId = Id;
            ViewBag.Id = Convert.ToInt32(User.FindFirst(ClaimTypes.Sid)?.Value);
            ViewBag.Name = User.FindFirst(ClaimTypes.Name)?.Value!.ToString();
            return View();
        }
    }
}
