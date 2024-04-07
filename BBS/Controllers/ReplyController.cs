using BBS.IService;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BBS.Controllers
{
    public class ReplyController(IReplyService replyService) : Controller
    {
        [Route("Reply/Reply/{PostId}")]
        public ActionResult Reply(string Content, int PostId)
        {
            int UserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Sid)?.Value);
            replyService.Reply(Content, UserId, PostId);
            return RedirectToRoute(new
            {
                controller = "Post",
                action = "GetPost",
                Id = PostId
            });
        }
    }
}
