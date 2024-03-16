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
                if (_postService.CreatePost(json.GetProperty("Title").ToString(), json.GetProperty("Content").ToString(), json.GetProperty("Tag").ToString(), ViewBag.Id))
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
        [HttpPost]
        [Route("Post/EditPost")]
        public ActionResult EditPost([FromBody] JsonElement json)
        {
            int PostId = json.GetProperty("PostId").GetInt32();
            string Title = json.GetProperty("Title").ToString();
            string Content = json.GetProperty("Content").ToString();
            if (_postService.GetPost(PostId).UserId != Convert.ToInt32(User.FindFirst(ClaimTypes.Sid)?.Value))
            {
                return RedirectToAction("GetPost", new { Id = PostId });
            }
            if (_postService.EditPost(PostId, Title, Content, json.GetProperty("Tag").ToString()))
            {
                return RedirectToAction("GetPost", new { Id = PostId });
            }
            return RedirectToAction("GetPost", new { Id = PostId });
        }
    }
}
