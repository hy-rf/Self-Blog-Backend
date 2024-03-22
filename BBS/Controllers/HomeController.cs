using BBS.Data;
using BBS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json;

namespace BBS.Controllers
{
    public class HomeController(AppDbContext ctx) : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
        public ActionResult Search(string Option, string SearchTerm)
        {
            switch (Option)
            {
                case "Title":
                    List<Post> ret = ctx.Post.Where(p => p.Title!.Contains(SearchTerm)).ToList();
                    return View(ret);
                case "Content":
                    List<Post> ret2 = ctx.Post.Where(p => p.Content!.Contains(SearchTerm)).ToList();
                    return View(ret2);
                case "Reply":
                    List<Reply> ret3 = ctx.Reply.Where(r => r.Content!.Contains(SearchTerm)).ToList();
                    return View(ret3);
            }
            return View("Index");
        }
        [HttpGet]
        [Route("search/{Option}/{SearchTerm}")]
        public List<string> SearchRecommendations(string Option, string SearchTerm)
        {
            HttpContext.Response.StatusCode = 200;
            switch (Option)
            {
                case "title":
                    List<string> ret = ctx.Post.Where(p => p.Title!.Contains(SearchTerm)).Select(p => p.Title).ToList();
                    return ret;
                case "content":
                    List<string> ret2 = ctx.Post.Where(p => p.Content!.Contains(SearchTerm)).Select(p => p.Content).ToList();
                    return ret2;
                case "reply":
                    List<string> ret3 = ctx.Reply.Where(r => r.Content!.Contains(SearchTerm)).Select(r => r.Content).ToList();
                    return ret3;
            }
            HttpContext.Response.StatusCode = 404;
            return null!;
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

