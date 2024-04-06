using BBS.Data;
using BBS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json;
using BBS.IService;
using System.Runtime.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using BBS.Hubs;

namespace BBS.Controllers
{
    public class FriendController(IFriendService friendService, INotificationService notificationService, IHubContext<BBS.Hubs.Notification> notification) : Controller
    {
        [HttpPost]
        [Route("Friend/{Id}")]
        public async Task<JsonResult> SendFriendRequest(int Id)
        {
            int SendUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Sid)?.Value);
            friendService.AddFriendRequest(new FriendRequest
            {
                SendUserId = SendUserId,
                ReceiveUserId = Id
            });
            Models.Notification newNotification = new()
            {
                UserId = Id,
                Type = "FriendRequest",
                Message = $"{SendUserId}",
                Url = $"/FriendRequests",
                IsRead = false
            };
            if (notificationService.AddNotification(newNotification).IsCompleted)
            {
                await notification.Clients.User(Id.ToString()).SendAsync("ReceiveNotification", newNotification);
                return Json(JsonBody.CreateResponse(true, "success"));
            }
            else
            {
                return Json(JsonBody.CreateResponse(false, "fail"));
            }
        }
        [HttpGet]
        [Route("FriendRequests")]
        public ActionResult FriendRequests()
        {
            int UserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Sid)!.Value);
            List<FriendRequest> ret = friendService.FriendRequests(UserId);
            return View("FriendRequests", ret);
        }
        [HttpGet]
        [Route("FriendRequestsSent")]
        public ActionResult FriendRequestSent()
        {
            int UserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Sid)!.Value);
            List<FriendRequest> ret = friendService.FriendRequestsSent(UserId);
            return View("FriendRequests", ret);
        }
        [HttpPost]
        [Route("Friend")]
        public void FriendRequestApprove([FromBody] JsonElement json)
        {
            int Id = json.GetProperty("Id").GetInt32();
            friendService.AddFriend(
                new Friend
                {
                    UserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Sid)!.Value),
                    FriendUserId = Id,
                },
                new Friend
                {
                    FriendUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Sid)!.Value),
                    UserId = Id,
                });
            friendService.RemoveFriendRequest(new FriendRequest
            {
                ReceiveUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Sid)!.Value),
                SendUserId = Id
            });
        }
        // API DONE
        [HttpGet]
        [Route("FriendList/{Id}")]
        public JsonResult GetFriendList(int Id)
        {
            var result = friendService.Friends(Id);
            return Json(new JsonBody
            {
                Success = true,
                Payload = result,
                Message = "Success"
            });
        }
    }
}
