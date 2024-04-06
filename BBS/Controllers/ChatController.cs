using BBS.IService;
using BBS.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace BBS.Controllers
{
    public class ChatController(IChatService chatService) : Controller
    {
        [HttpPost]
        [Route("api/ChatRoom")]
        public ActionResult CreateChatRoom([FromBody] ChatRoom chatRoom)
        {
            chatService.CreateChatRoom(chatRoom);
            chatService.AddMember(new ChatRoomMember
            {
                ChatRoomId = chatRoom.Id,
                UserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Sid)?.Value)
            });
            return Json(new JsonBody
            {
                Success = true,
                Message = "Chat room created"
            });
        }
        [HttpPost]
        [Route("api/GetJoinedChatRoom")]
        public JsonResult GetJoinedChatRoom()
        {
            try
            {
                int UserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Sid)?.Value);
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
        [HttpPost]
        [Route("api/GetChatRoomMessages")]
        public JsonResult GetChatRoomMessages([FromBody] JsonElement json)
        {
            try
            {
                int ChatRoomId = Convert.ToInt32(json.GetProperty("chatRoomId").GetString());
                var ret = chatService.GetChatMessagesSimple(ChatRoomId);
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
        [HttpGet]
        [Route("api/ChatRoomMember/{ChatRoomId}")]
        public JsonResult GetChatRoomMember(int ChatRoomId)
        {
            try
            {
                var ret = chatService.GetChatRoomMembers(ChatRoomId);
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
        [HttpPost]
        [Route("api/ChatRoomMember")]
        public JsonResult AddChatRoomMember([FromBody] JsonElement newMember)
        {
            try
            {
                chatService.AddMember(new Models.ChatRoomMember
                {
                    UserId = Convert.ToInt32(newMember.GetProperty("userId").GetString()),
                    ChatRoomId = Convert.ToInt32(newMember.GetProperty("ChatRoomId").GetString())
                });
                return Json(new JsonBody
                {
                    Success = true,
                    Payload = null,
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
        [HttpDelete]
        [Route("api/ChatRoomMember")]
        public JsonResult DeleteChatRoomMember([FromBody] JsonElement memberToKick)
        {
            try
            {
                chatService.KickMember(new Models.ChatRoomMember
                {
                    UserId = Convert.ToInt32(memberToKick.GetProperty("userId").GetString()),
                    ChatRoomId = Convert.ToInt32(memberToKick.GetProperty("ChatRoomId").GetString())
                });
                return Json(new JsonBody
                {
                    Success = true,
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
