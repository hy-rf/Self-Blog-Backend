using BBS.Data;
using BBS.Interfaces;
using BBS.Services;
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
using BBS.Models;

namespace BBS.Controllers
{
    public class UserController(IUserService userService, IConfiguration configuration, IFriendService friendService) : Controller
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
                string Id = User.FindFirst(ClaimTypes.Sid)!.Value;
                ViewBag.Id = Convert.ToInt32(Id);
                var model = userService.GetUser(ViewBag.Id);
                return View(model);
            }
            catch
            {
                return View("Index");
            }
        }
        [Route("User/{Id}")]
        public IActionResult UserPage(int Id)
        {
            int id = Convert.ToInt32(User.FindFirst(ClaimTypes.Sid)?.Value!);
            ViewBag.isFriend = friendService.isFriend(id, Id);
            ViewBag.isFriendRequestSent = friendService.isFriendRequestSent(id, Id);
            var model = userService.GetUser(Id);
            return View("UserPage", model);
        }
        /// <summary>
        /// DONE API
        /// </summary>
        /// <param name="Avatar"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("User/EditAvatar")]
        public ActionResult EditAvatar(IFormFile Avatar)
        {
            int Id = Convert.ToInt32(User.FindFirst(ClaimTypes.Sid)?.Value!);
            if (Avatar.Length > 0)
            {
                using MemoryStream ms = new MemoryStream();
                Avatar.CopyTo(ms);
                string s = Convert.ToBase64String(ms.ToArray());
                if (userService.EditAvatar(Id, s))
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
        // DONE API
        [HttpPost]
        [Route("User/EditName")]
        public void EditName([FromBody] JsonElement json)
        {
            int Id = Convert.ToInt32(User.FindFirst(ClaimTypes.Sid)?.Value!);
            string name = json.GetProperty("Name").ToString();
            if (userService.EditName(Id, name))
            {
                Response.StatusCode = 200;
                return;
            }
            Response.StatusCode = 404;
            return;
        }
        [Route("Logout")]
        public ActionResult Logout()
        {
            if (userService.Logoff(Convert.ToInt32(User.FindFirst(ClaimTypes.Sid)?.Value!)))
            {
                HttpContext.Response.Cookies.Delete("Token");
                return Redirect("/");
            }
            return Redirect("/UserCenter");
        }
        // API DONE NOT IN USE
        [HttpGet]
        [Route("api/User/{Id}")]
        public JsonResult UserJson(int Id)
        {
            var ret = userService.GetUserLight(Id);
            return Json(ret);
        }
        // API DONE
        [HttpPost]
        [Route("api/User/Login")]
        public JsonResult LoginApi([FromBody] JsonElement LoginInfo)
        {
            string Name = LoginInfo.GetProperty("Name").ToString();
            string Pwd = LoginInfo.GetProperty("Pwd").ToString();
            if (!string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Pwd))
            {
                if (userService.Login(Name, Pwd, out int Id))
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("JWT")));
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new[]
                        {
                                new Claim(ClaimTypes.Sid, Convert.ToString(Id)),
                                new Claim(ClaimTypes.Name, Name),
                                new Claim(ClaimTypes.Role, "User"),
                        // Add other claims as needed
                       }),
                        Expires = DateTime.Now.AddHours(5),
                        SigningCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256)
                    };
                    var tokenString = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
                    HttpContext.Response.Cookies.Append("Token", tokenString);
                    return Json(new JsonBody
                    {
                        Success = true,
                        Message = "Login Success!"
                    });
                }
                return Json(new JsonBody
                {
                    Success = false,
                    Message = "Login Failed, Wrong Name or Password."
                });
            }
            return Json(new JsonBody
            {
                Success = false,
                Message = "Login Failed, No Input."
            });
        }
        // API DONE
        [HttpPost]
        [Route("api/User/CheckDuplicatedName")]
        public JsonResult CheckDuplicatedName([FromBody] JsonElement Name)
        {
            string name = Name.GetProperty("Name").ToString();
            if (userService.CheckDuplicatedName(name))
            {
                return Json(new JsonBody
                {
                    Success = true,
                    Message = "Name is available"
                });
            }
            return Json(new JsonBody
            {
                Success = false,
                Message = "Name is not available"
            });
        }
        // API DONE
        [HttpPost]
        [Route("api/User/Signup")]
        public JsonResult SignupApi([FromBody] JsonElement SingupInfo)
        {
            string Name = SingupInfo.GetProperty("Name").ToString();
            string Pwd = SingupInfo.GetProperty("Pwd").ToString();
            string RePwd = SingupInfo.GetProperty("RePwd").ToString();
            if (!string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Pwd))
            {
                if (Pwd == RePwd)
                {
                    if (userService.Signup(Name, Pwd))
                    {
                        return Json(new JsonBody
                        {
                            Success = true,
                            Message = "Signup Success!"
                        });
                    }
                    else
                    {
                        return Json(new JsonBody
                        {
                            Success = false,
                            Message = "Signup Failed, Dulplicated Name."
                        });
                    }
                }
                return Json(new JsonBody
                {
                    Success = false,
                    Message = "Signup Failed, Passwords are not the same."
                });
            }
            return Json(new JsonBody
            {
                Success = false,
                Message = "Signup Failed, No Input."
            });
        }
    }
}
