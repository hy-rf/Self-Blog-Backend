using BBS.Interfaces;
using BBS.Models;
using BBS.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace BBS.Controllers
{
    public class PostController(IPostService postService) : Controller
    {
        [Route("/")]
        public IActionResult Index()
        {
            var model = postService.GetPosts();
            return View(model);
        }
        [HttpPost]
        [Route("Post/CreatePost")]
        public ActionResult CreatePost([FromBody] JsonElement json)
        {
            try
            {
                ViewBag.Id = Convert.ToInt32(User.FindFirst(ClaimTypes.Sid)?.Value);
                if (postService.CreatePost(json.GetProperty("Title").ToString(), json.GetProperty("Content").ToString(), json.GetProperty("Tag").ToString(), ViewBag.Id))
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
            return View("Post", postService.GetPost(Id));
        }
        [HttpPost]
        [Route("Post/EditPost")]
        public ActionResult EditPost([FromBody] JsonElement json)
        {
            int PostId = json.GetProperty("PostId").GetInt32();
            string Title = json.GetProperty("Title").ToString();
            string Content = json.GetProperty("Content").ToString();
            if (postService.GetPost(PostId).UserId != Convert.ToInt32(User.FindFirst(ClaimTypes.Sid)?.Value))
            {
                return BadRequest();
            }
            if (postService.EditPost(PostId, Title, Content, json.GetProperty("Tag").ToString()))
            {
                return RedirectToAction("GetPost", new { Id = PostId });
            }
            return RedirectToAction("GetPost", new { Id = PostId });
        }
    }
}
