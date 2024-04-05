using BBS.Hubs;
using BBS.Interfaces;
using BBS.Models;
using BBS.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using System.Text.Json;

namespace BBS.Controllers
{

    public class PostController(IPostService postService, IFriendService friendService, INotificationService notificationService, IHubContext<Hubs.Notification> notification) : Controller
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
        public async Task<IActionResult> CreatePost([FromBody] JsonElement json)
        {
            // try
            // {
            int Id;
            ViewBag.Id = Convert.ToInt32(User.FindFirst(ClaimTypes.Sid)?.Value);
            if (postService.CreatePost(json.GetProperty("Title").ToString(), json.GetProperty("Content").ToString(), json.GetProperty("Tag").ToString(), ViewBag.Id, out Id))
            {
                IAsyncEnumerable<Friend> Friend = friendService.Friends(ViewBag.Id);
                await foreach (var item in Friend)
                {
                    // await notificationService.AddNotification(new BBS.Models.Notification
                    // {
                    //     UserId = item.FriendUser.Id,
                    //     Type = "Post",
                    //     Message = $"Friend {ViewBag.Id} created a new post: {json.GetProperty("Title")}",
                    //     Url = $"/Post/{Id}",
                    //     IsRead = false
                    // });
                    _ = notification.Clients.User(item.FriendUser.Id.ToString()).SendAsync("ReceiveNotification", new BBS.Models.Notification
                    {
                        UserId = item.FriendUser.Id,
                        Type = "Post",
                        Message = $"Friend {ViewBag.Id} created a new post: {json.GetProperty("Title")}",
                        Url = $"/Post/{Id}",
                        IsRead = false
                    });
                }
                return RedirectToAction("Index");
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
        public JsonResult GetPostContentAPI(int Id)
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
        [HttpPost]
        [Route("api/Post")]
        public JsonResult GetPost()
        {
            var result = postService.GetPostsLite();
            return Json(JsonBody.CreateResponse(true, result, "success"));
        }
    }
}
