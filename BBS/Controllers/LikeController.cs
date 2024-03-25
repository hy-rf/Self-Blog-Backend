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
            likeService.AddLikePost(likedPost);
            return Ok();
        }
    }
}
