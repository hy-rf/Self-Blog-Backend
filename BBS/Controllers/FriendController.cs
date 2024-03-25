using BBS.Data;
using BBS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json;
using BBS.Interfaces;
using System.Runtime.Versioning;
using Microsoft.AspNetCore.Authorization;

namespace BBS.Controllers
{
    public class FriendController(IFriendService friendService) : Controller
    {
        [HttpPost]
        [Route("Friend/{Id}")]
        public void SendFriendRequest(int Id)
        {
            friendService.AddFriendRequest(new FriendRequest
            {
                SendUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Sid)?.Value),
                ReceiveUserId = Id
            });
        }
        [HttpGet]
        [Authorize]
        [Route("FriendRequests")]
        public ActionResult FriendRequests()
        {
            int UserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Sid)!.Value);
            List<FriendRequest> ret = friendService.FriendRequests(UserId);
            return View("FriendRequests", ret);
        }
        [HttpGet]
        [Authorize]
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
        [HttpGet]
        [Route("FriendList/{Id}")]
        public JsonResult GetFriendList(int Id)
        {
            //var friends = ctx.Friend.Where(f => f.UserId == Id);
            //var ret = from f in ctx.Friend.Where(f => f.UserId == Id)
            //          join user in ctx.User on f.FriendUserId equals user.Id
            //          select new
            //          {
            //              f.FriendUserId,
            //              user.Id,
            //              user.Name,
            //              user.Created,
            //              user.LastLogin,
            //              user.Avatar
            //          };
            var result = friendService.Friends(Id);
            //var ret = JsonSerializer.Serialize(result);
            return Json(result);
        }
    }
}
