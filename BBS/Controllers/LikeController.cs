using BBS.Interfaces;
using BBS.Models;
using Microsoft.AspNetCore.Mvc;

namespace BBS.Controllers
{
    public class LikeController(ILikeService likeService) : Controller
    {
        [HttpPost]
        [Route("/Like/Post")]
        public ActionResult LikePost([FromBody]LikedPost likedPost)
        {
            if (User.Identity!.IsAuthenticated == false)
            {
                return Ok();
            }
            likeService.AddLikePost(likedPost);
            return Ok();
        }
    }
}
