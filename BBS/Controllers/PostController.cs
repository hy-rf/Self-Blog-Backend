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
        [Route("Post/{page}")]
        public IActionResult Index(int page)
        {
            ViewBag.NumPost = postService.CountPost();
            ViewBag.PostPerPage = 10;
            var model = postService.GetPostsByPage(page, 10);
            return View(model);
        }
        // API DONE
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
            var ret = postService.GetPost(Id);
            return View("Post", ret);
        }
        [HttpGet]
        [Route("api/Post/{Id}")]
        public JsonResult GetPostAPI(int Id)
        {
            var ret = postService.GetPost(Id).Content;
            return Json(JsonBody.CreateResponse(true, ret, "success"));
        }
        // API DONE
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
