using BBS.Common;
using BBS.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace BBS.Controllers
{
    public class NotificationController : Controller
    {
        [HttpGet("Notifications")]
        public async Task<JsonResult> Notifications(int UserId)
        {
            return Json(JsonBody.CreateResponse(false, "Feature Not Implemented"));
        }
        [HttpDelete("Notifications")]
        public async Task<JsonResult> Delete([FromBody] Notification notification)
        {
            return Json(JsonBody.CreateResponse(false, "Feature Not Implemented"));
        }
    }
}
