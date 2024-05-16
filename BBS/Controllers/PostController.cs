using BBS.Common;
using BBS.Data;
using BBS.IService;
using BBS.Models;
using BBS.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json;

namespace BBS.Controllers
{

    public class PostController(IPostService postService, IFriendService friendService, INotificationService notificationService, IHubContext<Hubs.Notification> notification, ForumContext ctx) : Controller
    {
        [Route("Post/{page}")]
        public IActionResult Index(int page)
        {
            ViewBag.NumPost = postService.CountPost();
            ViewBag.PostPerPage = 10;
            var model = postService.GetPostsByPage(page, 10);
            return View(model);
        }

        [HttpPost]
        [Route("Post/CreatePost")]
        public async Task<JsonResult> CreatePost([FromBody] JsonElement json)
        {
            if (string.IsNullOrEmpty(json.GetProperty("Title").ToString()) || string.IsNullOrEmpty(json.GetProperty("Content").ToString()))
            {
                return Json(JsonBody.CreateResponse(false, "Title, Content cannot be empty"));
            }
            ViewBag.Id = Convert.ToInt32(User.FindFirst(ClaimTypes.Sid)?.Value);
            if (postService.CreatePost(json.GetProperty("Title").ToString(), json.GetProperty("Content").ToString(), json.GetProperty("Tag").ToString(), ViewBag.Id).IsCompleted)
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
                        Url = $"/Post/Detail/{postService.CountPost()}",
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
        [Route("/Post/Content/{Id}")]
        public JsonResult GetPostContentAPI(int Id)
        {
            var ret = postService.GetPost(Id).Result.Content;
            return Json(JsonBody.CreateResponse(true, ret, "success"));
        }

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
            catch
            {
                return Json(JsonBody.CreateResponse(false, "Unauthorized Access"));
            }

            if (postService.EditPost(PostId, Title, Content, json.GetProperty("Tag").ToString()).Result)
            {
                return Json(JsonBody.CreateResponse(true, "Edit Post Success"));
            }
            return Json(JsonBody.CreateResponse(false, "Internal Server Error"));
        }









        [HttpGet("/post")]
        public JsonResult GetPosts()
        {
            List<PostListViewModel> result = ctx.Post.Include(p => p.User).Include(p => p.PostTags).ThenInclude(pt => pt.Tag).Include(p => p.Likes).ThenInclude(l => l.User).Select(p => new PostListViewModel
            {
                Id = p.Id,
                Title = p.Title,
                ContentPreview = p.Content,
                Created = p.Created,
                Modified = p.Modified,
                UserId = p.User.Id,
                UserName = p.User.Name,
                Tags = p.PostTags.Select(pt => pt.Tag).ToList(),
                LikeUsers = p.Likes.Select(l => new UserBriefViewModel
                {
                    Id = l.User.Id,
                    Name = l.User.Name,
                    Created = l.User.Created,
                    Avatar = l.User.Avatar
                }).ToList()
            }).ToList();
            return Json(JsonBody.CreateResponse(true, result, "success"));
        }
        [HttpGet("/post/detail")]
        public JsonResult GetPostDetail(int id)
        {
            PostDetailViewModel result = ctx.Post.Include(p => p.User).Include(p => p.PostTags).ThenInclude(pt => pt.Tag).Include(p => p.Likes).ThenInclude(l => l.User).Include(p => p.Replies).ThenInclude(r => r.User).Select(p => new PostDetailViewModel
            {
                Id = id,
                Title = p.Title,
                Content = p.Content,
                Created = p.Created,
                Updated = p.Modified,
                UserId = p.UserId,
                UserName = p.User.Name,
                Tags = p.PostTags.Select(pt => pt.Tag).ToList(),
                LikeUsers = p.Likes.Select(l => new UserBriefViewModel
                {
                    Id = l.User.Id,
                    Name = l.User.Name,
                    Created = l.User.Created,
                    Avatar = l.User.Avatar
                }).ToList(),
                NumberOfLikes = p.Likes.Count(),
                NumberOfReplies = p.Replies.Count(),
            }).Single(p => p.Id == id);
            return Json(JsonBody.CreateResponse(true, result, "success"));
        }
        [HttpPost("/post")]
        public JsonResult CreatePost(CreatePostViewModel createPostViewModel)
        {
            throw new NotImplementedException();
        }
        [HttpPut("/post")]
        public JsonResult UpdatePost(EditPostViewModel editPostViewModel)
        {
            throw new NotImplementedException();
        }
    }
}
