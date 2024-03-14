using Microsoft.AspNetCore.Mvc;
using BBS.Interfaces;
using BBS.Models;
using System.Security.Claims;

namespace BBS.Controllers
{
    public class ReplyController : Controller
    {
        private readonly IReplyService _replyService;
        public ReplyController(IReplyService replyService)
        {
            _replyService = replyService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [Route("Reply/Reply/{PostId}")]
        public ActionResult Reply(string Content, int PostId)
        {
            int UserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Sid)?.Value);
            _replyService.Reply(Content, UserId, PostId);
            return RedirectToRoute(new
            {
                controller = "Post",
                action = "GetPost",
                Id = PostId
            });
        }
    }
}
