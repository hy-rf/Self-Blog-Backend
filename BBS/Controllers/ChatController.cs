using BBS.Interfaces;
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
        public IActionResult Index()
        {
            ViewBag.Id = Convert.ToInt32(User.FindFirst(ClaimTypes.Sid)?.Value);
            ViewBag.Name = User.FindFirst(ClaimTypes.Name)?.Value!.ToString();
            var ret = chatService.GetChatRooms(Convert.ToInt32(User.FindFirst(ClaimTypes.Sid)?.Value));
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
        public JsonResult GetChatRooms([FromBody] JsonElement json)
        {
            int UserId = Convert.ToInt32(json.GetProperty("Id").GetString());
            var ret = chatService.GetJoinedChatRooms(UserId);
            return Json(ret.Select(cr => new {cr.Id, cr.Name}));
        }


        [HttpPost]
        public void AddChatRoomMember([FromBody] JsonElement json)
        {
            int UserId = Convert.ToInt32(json.GetProperty("UserId").GetString());
            int ChatRoomId = Convert.ToInt32(json.GetProperty("ChatRoomId").GetString());
            chatService.AddMember(new Models.ChatRoomMember
            {
                UserId = UserId,
                ChatRoomId = ChatRoomId
            });
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
    }
}
