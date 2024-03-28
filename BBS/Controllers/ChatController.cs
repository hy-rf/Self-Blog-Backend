using BBS.Interfaces;
using BBS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace BBS.Controllers
{
    public class ChatController(IChatService chatService) : Controller
    {
        [Authorize]
        [Route("Chat/Index")]
        public IActionResult Index()
        {
            ViewBag.Id = Convert.ToInt32(User.FindFirst(ClaimTypes.Sid)?.Value);
            ViewBag.Name = User.FindFirst(ClaimTypes.Name)?.Value!.ToString();
            var ret = chatService.GetJoinedChatRooms(Convert.ToInt32(User.FindFirst(ClaimTypes.Sid)?.Value));
            return View(ret);
        }
        [Authorize]
        [Route("ChatRoom/{Id}")]
        public IActionResult ChatRoom(int Id)
        {
            if (chatService.isInChatRoom(new Models.ChatRoomMember
            {
                Id = -1,
                UserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Sid)?.Value),
                ChatRoomId = Id,
            }))
            {
                ViewBag.ChatRoomId = Id;
                ViewBag.Id = Convert.ToInt32(User.FindFirst(ClaimTypes.Sid)?.Value);
                ViewBag.Name = User.FindFirst(ClaimTypes.Name)?.Value!.ToString();
                return View(chatService.GetChatMessages(Id));
            }
            return Redirect("/");
        }
        [HttpPost]
        [Route("Chat/CreateChatRoom")]
        public void CreateChatRoom(string Name)
        {
            var newchatroom = new Models.ChatRoom
            {
                Name = Name
            };
            chatService.CreateChatRoom(newchatroom);
            chatService.AddMember(new Models.ChatRoomMember
            {
                ChatRoomId = newchatroom.Id,
                UserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Sid)?.Value)
            });
        }
        [HttpPost]
        [Route("Chat/GetChatRooms")]
        public JsonResult GetChatRooms([FromBody] JsonElement json)
        {
            int UserId = Convert.ToInt32(json.GetProperty("Id").GetString());
            var ret = chatService.GetJoinedChatRooms(UserId);
            return Json(ret.Select(cr => new { cr.Id, cr.Name }));
        }


        [HttpPost]
        [Route("Chat/AddChatRoomMember")]
        public JsonResult AddChatRoomMember([FromBody] JsonElement json)
        {
            try
            {
                int UserId = Convert.ToInt32(json.GetProperty("UserId").GetString());
                int ChatRoomId = Convert.ToInt32(json.GetProperty("ChatRoomId").GetString());
                chatService.AddMember(new Models.ChatRoomMember
                {
                    UserId = UserId,
                    ChatRoomId = ChatRoomId
                });
                return Json(new JsonBody
                {
                    Success = true,
                    Message = "Success"
                });
            }
            catch (Exception e)
            {
                return Json(new JsonBody
                {
                    Success = false,
                    Message = e.Message
                });
            }

        }
        [HttpDelete]
        [Route("Chat/KickChatRoomMember")]
        public void KickChatRoomMember([FromBody] JsonElement json)
        {
            int UserId = Convert.ToInt32(json.GetProperty("UserId").GetString());
            int ChatRoomId = Convert.ToInt32(json.GetProperty("ChatRoomId").GetString());
            chatService.KickMember(new Models.ChatRoomMember
            {
                UserId = UserId,
                ChatRoomId = ChatRoomId
            });
        }
        // API DONE
        [HttpPost]
        [Route("apt/GetJoinedChatRoom")]
        public JsonResult GetJoinedChatRoom([FromBody] JsonElement json)
        {
            try
            {
                int UserId = Convert.ToInt32(json.GetProperty("Id").GetString());
                var ret = chatService.GetJoinedChatRooms(UserId);
                return Json(new JsonBody
                {
                    Success = true,
                    Payload = ret,
                    Message = "Success"
                });
            }
            catch
            {
                return Json(new JsonBody
                {
                    Success = false,
                    Message = "Error"
                });
            }
        }
    }
}
