using BBS.Interfaces;
using BBS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Security.Claims;
using System.Text.Json;

namespace BBS.Controllers
{
    public class LikeController(ILikeService likeService) : Controller
    {
        // API DONE
        /// <summary>
        /// Post JSON stringified object as LikedPost backend object
        /// </summary>
        /// <param name="likedPost"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("/Like/Post")]
        public JsonResult LikePost([FromBody] LikedPost likedPost)
        {
            if (User.Identity!.IsAuthenticated == true)
            {
                try
                {
                    likeService.AddLikePost(new LikedPost
                    {
                        UserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Sid)?.Value),
                        PostId = likedPost.PostId
                    });
                    return Json(new JsonBody
                    {
                        Success = true,
                        Message = "Post liked"
                    });
                }
                catch
                {
                    return Json(new JsonBody
                    {
                        Success = true,
                        Message = "Post already liked"
                    });
                }
                finally
                {
                    Console.WriteLine($"Like post request received at {DateTime.Now}");
                }
            }
            return Json(new JsonBody
            {
                Success = true,
                Message = "Not Authorized"
            });
        }
    }
}
