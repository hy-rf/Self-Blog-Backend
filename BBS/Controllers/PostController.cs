using BBS.IService;
using BBS.Models;
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
        public async Task<JsonResult> CreatePost([FromBody] JsonElement json)
        {
            if (string.IsNullOrEmpty(json.GetProperty("Title").ToString()) || string.IsNullOrEmpty(json.GetProperty("Content").ToString()))
            {
                return Json(JsonBody.CreateResponse(false, "Title, Content cannot be empty"));
            }
            ViewBag.Id = Convert.ToInt32(User.FindFirst(ClaimTypes.Sid)?.Value);
            if (postService.CreatePost(json.GetProperty("Title").ToString(), json.GetProperty("Content").ToString(), json.GetProperty("Tag").ToString(), ViewBag.Id, out int Id).IsCompleted)
            {
                // Send notification to all friends
                IAsyncEnumerable<Friend> Friend = friendService.Friends(ViewBag.Id);
                await foreach (var item in Friend)
                {
                    Models.Notification newNotification = new()
                    {
                        UserId = item.FriendUser.Id,
                        Type = "Post",
                        Message = $"{ViewBag.Id}",
                        Url = $"/Post/Detail/{Id}",
                        IsRead = false
                    };
                    await notificationService.AddNotification(newNotification);
                    await notification.Clients.User(item.FriendUser.Id.ToString()).SendAsync("ReceiveNotification", newNotification);
                }
                return Json(JsonBody.CreateResponse(true, "Create Post Success"));
            }
            return Json(JsonBody.CreateResponse(false, "Internal Server Error"));
        }
        [Route("Post/Detail/{Id}")]
        public ActionResult GetPost(int Id)
        {
            var ret = postService.GetPost(Id).Result;
            return View("Post", ret);
        }
        [HttpGet]
        [Route("api/Post/{Id}")]
        public JsonResult GetPostContentAPI(int Id)
        {
            var ret = postService.GetPost(Id).Result.Content;
            return Json(JsonBody.CreateResponse(true, ret, "success"));
        }
        // API DONE
        [HttpPost]
        [Route("Post/EditPost")]
        public JsonResult EditPost([FromBody] JsonElement json)
        {
            if (string.IsNullOrEmpty(json.GetProperty("Title").ToString()) || string.IsNullOrEmpty(json.GetProperty("Content").ToString()))
            {
                return Json(JsonBody.CreateResponse(false, "Title, Content cannot be empty"));
            }
            int PostId = json.GetProperty("PostId").GetInt32();
            string Title = json.GetProperty("Title").ToString();
            string Content = json.GetProperty("Content").ToString();
            try
            {
                if (postService.GetPost(PostId).Result.UserId != Convert.ToInt32(User.FindFirst(ClaimTypes.Sid)?.Value))
                {
                    return Json(JsonBody.CreateResponse(false, "Unauthorized Access"));
                }
            }
            catch{
                return Json(JsonBody.CreateResponse(false, "Unauthorized Access"));
            }

            if (postService.EditPost(PostId, Title, Content, json.GetProperty("Tag").ToString()))
            {
                return Json(JsonBody.CreateResponse(true, "Edit Post Success"));
            }
            return Json(JsonBody.CreateResponse(false, "Internal Server Error"));
        }
        [HttpPost]
        [Route("api/Post")]
        public JsonResult GetPost()
        {
            return Json(JsonBody.CreateResponse(true, "success"));
        }
    }
}
