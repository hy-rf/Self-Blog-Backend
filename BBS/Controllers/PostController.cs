using BBS.Interfaces;
using BBS.Models;
using Microsoft.AspNetCore.Mvc;

namespace BBS.Controllers
{
    public class PostController : Controller
    {
        private readonly IPostService _postService;
        public PostController(IPostService postService)
        {
            _postService = postService;
        }
        public IActionResult Index()
        {
            List<Post> posts = _postService.GetRecentPosts();
            ViewBag.Posts = posts;
            return View();
        }
        public ActionResult CreatePost(string Title, string Content, string? Tags)
        {
            try
            {
                ViewBag.Id = HttpContext.Session.GetInt32("Id");
                if (_postService.CreatePost(Title, Content, ViewBag.Id, Tags))
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
        [Route("Post/EditPost/{PostId}")]
        public ActionResult EditPost(int PostId, string Title, string Content, string Tags){
            if (_postService.EditPost(PostId, Title, Content, Tags)){
                System.Diagnostics.Debug.WriteLine("Success");
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
}
