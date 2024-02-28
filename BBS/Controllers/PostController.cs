using BBS.Interfaces;
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
            return View();
        }
        public ActionResult CreatePost(string Title, string Content, string? Tags)
        {

            ViewBag.Id = HttpContext.Session.GetInt32("Id");
            if(_postService.CreatePost(Title, Content, ViewBag.Id, Tags)){
                System.Diagnostics.Debug.WriteLine("Post Success");
            }
            System.Diagnostics.Debug.WriteLine("Post Fail");
            return View("Index");
        }
    }
}
