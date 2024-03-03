using Microsoft.AspNetCore.Mvc;
using BBS.Interfaces;
using BBS.Models;

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
            int UserId = (int)HttpContext.Session.GetInt32("Id");
            string UserName = (string)HttpContext.Session.GetString("Name");
            _replyService.Reply(Content, UserId, PostId);
            return RedirectToRoute(new
            {
                controller = "Post",
                action = "Index"
            });
        }
    }
}
