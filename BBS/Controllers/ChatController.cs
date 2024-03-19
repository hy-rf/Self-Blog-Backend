using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BBS.Controllers
{
    public class ChatController : Controller
    {
        [Authorize]
        public IActionResult Index()
        {
            ViewBag.Id = Convert.ToInt32(User.FindFirst(ClaimTypes.Sid)?.Value);
            ViewBag.Name = User.FindFirst(ClaimTypes.Name)?.Value!.ToString();
            return View();
        }
    }
}
