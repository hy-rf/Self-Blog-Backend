using BBS.Interfaces;
using BBS.Models;
using BBS.Services;
using Microsoft.AspNetCore.Mvc;

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
            List<Post> posts = _postService.GetPosts();
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
        [Route("Post/Detail/{Id}")]
        public ActionResult GetPost(int Id)
        {
            ViewBag.Replies = _replyService.GetReplies(Id);
            ViewBag.Post = _postService.GetPost(Id);
            return View("Post");
        }
        [Route("Post/EditPost/{PostId}")]
        public ActionResult EditPost(int PostId, string Title, string Content, string Tags){
            if (_postService.EditPost(PostId, Title, Content, Tags)){
                return RedirectToAction("GetPost", new {Id = PostId });
            }
            return RedirectToAction("GetPost", new { Id = PostId });
        }
    }
}
