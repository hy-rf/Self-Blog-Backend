using BBS.Data;
using BBS.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace BBS.Controllers
{
    public class UserController(IUserService _userService, IConfiguration configuration, AppDbContext ctx) : Controller
    {
        [Route("Welcome")]
        public IActionResult Index()
        {
            if (User.Identity!.IsAuthenticated)
            {

                return RedirectToAction("UserCenter");
            }
            return View();
        }
        [Route("UserCenter")]
        public IActionResult UserCenter(ClaimsPrincipal claimsPrincipal)
        {
            try
            {
                string Id = User.FindFirst(ClaimTypes.Sid)?.Value;
                ViewBag.Id = Convert.ToInt32(Id);
                var model = _userService.GetUser(ViewBag.Id);
                return View(model);
            }
            catch
            {
                return Unauthorized();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public IActionResult UserPage(int Id)
        {
            string id = User.FindFirst(ClaimTypes.Sid)?.Value!;
            var isFriend = ctx.Friend.Any(f => f.UserId == Convert.ToInt32(id) && f.FriendUserId == Id);
            ViewBag.isFriend = isFriend;
            var model = _userService.GetUser(Id);
            return View(model);
        }
        public ActionResult Signup(string Name, string Pwd)
        {
            if (!string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Pwd))
            {
                if (_userService.Signup(Name, Pwd))
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }
        public ActionResult Login(string Name, string Pwd)
        {
            if (!string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Pwd))
            {
                if (_userService.Login(Name, Pwd, out int Id))
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("JWT")));
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new[]
                        {
                                new Claim(ClaimTypes.Sid, Convert.ToString(Id)),
                                new Claim(ClaimTypes.Name, Name),
                        // Add other claims as needed
                       }),
                        Expires = DateTime.Now.AddHours(5),
                        SigningCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256)
                    };
                    var tokenString = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
                    HttpContext.Response.Cookies.Append("Token", tokenString);
                    return RedirectToAction("UserCenter");
                }
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        [Route("User/EditAvatar/{Id}")]
        public ActionResult EditAvatar(int Id, IFormFile Avatar)
        {
            if (Avatar.Length > 0)
            {
                using MemoryStream ms = new MemoryStream();
                Avatar.CopyTo(ms);
                string s = Convert.ToBase64String(ms.ToArray());
                if (_userService.EditAvatar(Id, s))
                {
                    return RedirectToAction("UserCenter");
                }
            }
            else
            {
                return RedirectToAction("UserCenter");
            }
            return RedirectToAction("UserCenter");
        }
        [HttpPost]
        [Route("User/EditName/{Id}")]
        public void EditName(int Id, [FromBody] JsonElement json)
        {
            string name = json.GetProperty("Name").ToString();
            if (_userService.EditName(Id, name))
            {
                Response.StatusCode = 200;
                return;
            }
            Response.StatusCode = 404;
            return;
        }
        public ActionResult Logout()
        {
            HttpContext.Response.Cookies.Delete("Token");
            return RedirectToAction("Index");
        }
        [HttpGet]
        [Route("User/{Id}")]
        public JsonResult UserJson(int Id)
        {
            //HttpContext.Response.Headers.Append("Access-Control-Allow-Origin", "*");
            var ret = _userService.GetUserLight(Id);
            return Json(ret);
        }
        
    }
}
