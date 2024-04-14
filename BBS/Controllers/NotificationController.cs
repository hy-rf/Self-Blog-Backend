using BBS.Common;
using BBS.IService;
using BBS.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BBS.Controllers
{
    public class NotificationController(INotificationService notificationService) : Controller
    {
        [HttpGet("Notifications")]
        public async Task<JsonResult> Notifications()
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return Json(JsonBody.CreateResponse(false, "Unauthorized"));
            }
            int UserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Sid).Value);
            var ret = notificationService.GetAllNotifications(UserId).Result;
            return Json(JsonBody.CreateResponse(true, ret, "Get notification success"));
        }
        [HttpDelete("Notifications")]
        public async Task<JsonResult> Delete([FromBody] Notification notification)
        {
            return Json(JsonBody.CreateResponse(false, "Feature Not Implemented"));
        }
    }
}
