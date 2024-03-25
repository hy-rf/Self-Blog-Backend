using BBS.Interfaces;
using BBS.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace BBS.Controllers
{
    public class LikeController(ILikeService likeService) : Controller
    {
        [HttpPost]
        [Route("/Like/Post")]
        public JsonResult LikePost([FromBody]LikedPost likedPost)
        {
            if (User.Identity!.IsAuthenticated == false)
            {
                return Json(new LikedPost
                {
                    UserId = 0,
                    PostId = 0
                });
            }
            likeService.AddLikePost(likedPost);
            return Json(likedPost);
        }
    }
}
