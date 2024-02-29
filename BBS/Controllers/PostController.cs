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

            ViewBag.Id = HttpContext.Session.GetInt32("Id");

            if (_postService.CreatePost(Title, Content, ViewBag.Id, Tags))
            {
                System.Diagnostics.Debug.WriteLine("Post Success");
            }
            return RedirectToAction("Index");
        }
    }
}
