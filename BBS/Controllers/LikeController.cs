﻿using BBS.IService;
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
                catch{
                    return Json(new JsonBody{
                        Success = true,
                        Message = "De-Liked Post"
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
