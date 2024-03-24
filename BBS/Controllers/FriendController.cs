using BBS.Data;
using BBS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json;

namespace BBS.Controllers
{
    public class FriendController(AppDbContext ctx) : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [Route("Friend/{Id}")]
        public void SendFriendRequest(int Id)
        {
            ctx.FriendRequest.Add(new FriendRequest
            {
                SendUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Sid)?.Value),
                ReceiveUserId = Id
            });
            ctx.SaveChanges();
        }
        [HttpGet]
        [Route("FriendRequests")]
        public ActionResult FriendRequests()
        {
            var ret = ctx.FriendRequest.Where(fr => fr.ReceiveUserId == Convert.ToInt32(User.FindFirst(ClaimTypes.Sid)!.Value)).Include(fr => fr.SendUser).ToList();
            return View("FriendRequests", ret);
        }
        [HttpGet]
        [Route("FriendRequestsSent")]
        public ActionResult FriendRequestSent()
        {
            var ret = ctx.FriendRequest.Where(fr => fr.SendUserId == Convert.ToInt32(User.FindFirst(ClaimTypes.Sid)!.Value)).Include(fr => fr.ReceiveUser).ToList();
            var test = ret.GetType();
            return View("FriendRequests", ret);
        }
        [HttpPost]
        [Route("Friend")]
        public void FriendRequestApprove([FromBody] JsonElement json)
        {
            int Id = json.GetProperty("Id").GetInt32();
            var newfriend = new Friend
            {
                UserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Sid)!.Value),
                FriendUserId = Id,
            };
            ctx.Friend.Add(newfriend);
            var newfriendopposite = new Friend
            {
                FriendUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Sid)!.Value),
                UserId = Id,
            };
            var reqtorm = ctx.FriendRequest.Where(fr => fr.ReceiveUserId == Convert.ToInt32(User.FindFirst(ClaimTypes.Sid)!.Value) && fr.SendUserId == Id).Single();
            ctx.FriendRequest.Remove(reqtorm);
            ctx.Friend.Add(newfriendopposite);
            ctx.SaveChanges();
        }
        [HttpGet]
        [Route("FriendList/{Id}")]
        public JsonResult GetFriendList(int Id)
        {
            var friends = ctx.Friend.Where(f => f.UserId == Id);
            var ret = from f in ctx.Friend.Where(f => f.UserId == Id)
                      join user in ctx.User on f.FriendUserId equals user.Id
                      select new
                      {
                          f.FriendUserId,
                          user.Id,
                          user.Name,
                          user.Created,
                          user.LastLogin,
                          user.Avatar
                      };
            return Json(ret.ToList());
        }
    }
}
