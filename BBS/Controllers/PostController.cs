using BBS.Interfaces;
using BBS.Models;
using BBS.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace BBS.Controllers
{
    public class PostController(IPostService _postService, IReplyService _replyService) : Controller
    {
        public IActionResult Index()
        {
            var model = _postService.GetPosts();
            return View(model);
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
            //ViewBag.Replies = _replyService.GetReplies(Id);
            //ViewBag.Post = _postService.GetPost(Id);
            return View("Post", _postService.GetPost(Id));
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
