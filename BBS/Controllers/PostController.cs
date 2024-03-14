using BBS.Interfaces;
using BBS.Models;
using BBS.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace BBS.Controllers
{
    public class PostController : Controller
    {
        private readonly IPostService _postService;
        private readonly IReplyService _replyService;
        public PostController(IPostService postService, IReplyService replyService)
        {
            _postService = postService;
            _replyService = replyService;
        }
        public IActionResult Index()
        {
            ViewBag.Posts = _postService.GetPosts();
            return View();
        }
        [HttpPost]
        [Route("Post/CreatePost")]
        public ActionResult CreatePost([FromBody] JsonElement json)
        {
            try
            {
                ViewBag.Id = Convert.ToInt32(User.FindFirst(ClaimTypes.Sid)?.Value);
                if (_postService.CreatePost(json.GetProperty("Title").ToString(), json.GetProperty("Content").ToString(), ViewBag.Id))
                {
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                return RedirectToRoute(new
                {
                    controller = "User",
                    action = "Index"
                });
            }
            return RedirectToAction("Index");
        }
        [Route("Post/Detail/{Id}")]
        public ActionResult GetPost(int Id)
        {
            ViewBag.Replies = _replyService.GetReplies(Id);
            ViewBag.Post = _postService.GetPost(Id);
            return View("Post");
        }
        [Route("Post/EditPost/{PostId}")]
        public ActionResult EditPost(int PostId, string Title, string Content){
            if (_postService.EditPost(PostId, Title, Content)){
                return RedirectToAction("GetPost", new {Id = PostId });
            }
            return RedirectToAction("GetPost", new { Id = PostId });
        }
    }
}
