using BBS.Common;
using BBS.IService;
using Google.Authenticator;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace BBS.Controllers
{
    public class UserController(IUserService userService, IConfiguration configuration, IFriendService friendService, ILogger<UserController> logger) : Controller
    {
        [Route("Welcome")]
        public IActionResult Index()
        {
            logger.LogError($"{DateTime.Now} : {this.GetType()} : {HttpContext.Connection.RemoteIpAddress} : Use Welcome");
            if (User.Identity!.IsAuthenticated)
            {

                return RedirectToAction("UserCenter");
            }
            return View();
        }
        [Route("UserCenter")]
        public IActionResult UserCenter(ClaimsPrincipal claimsPrincipal)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Redirect("/Welcome");
            }
            string Id = User.FindFirst(ClaimTypes.Sid)!.Value;
            ViewBag.Id = Convert.ToInt32(Id);
            var model = userService.GetUser(ViewBag.Id);
            return View(model);
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



        // DONE API
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

        [HttpGet]
        [Route("api/User/Avatar/{Id?}")]
        public JsonResult UserAvatar(int Id)
        {
            if (Id == 0)
            {
                Id = Convert.ToInt32(User.FindFirst(ClaimTypes.Sid)?.Value!);
            }
            try
            {
                return Json(JsonBody.CreateResponse(true, userService.GetUserBasic(Id).Avatar, "Avatar load success"));
            }
            catch
            {
                return Json(JsonBody.CreateResponse(false, "Avatar load fail"));
            }

        }
        [HttpGet]
        [Route("api/User/{Id?}")]
        public PartialViewResult UserInfo(int Id)
        {
            if (Id == 0)
            {
                Id = Convert.ToInt32(User.FindFirst(ClaimTypes.Sid)?.Value!);
            }
            return PartialView(userService.GetUserBasic(Id));
        }

        [HttpPost]
        [Route("api/User/Login")]
        public JsonResult Login([FromBody] JsonElement LoginInfo)
        {
            string Name = LoginInfo.GetProperty("Name").ToString();
            string Pwd = LoginInfo.GetProperty("Pwd").ToString();
            if (!string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Pwd))
            {
                if (userService.Login(Name, Pwd, out int Id).Result)
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
                    HttpContext.Response.Cookies.Append("Token", tokenString, new CookieOptions
                    {
                        Secure = true,
                        HttpOnly = true,
                        SameSite = SameSiteMode.None
                    });
                    return Json(new JsonBody
                    {
                        Success = true,
                        Payload = tokenString,
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

        [HttpPost]
        [Route("api/User/Signup")]
        public JsonResult Signup([FromBody] JsonElement SingupInfo)
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
                        return Json(JsonBody.CreateResponse(true, "Signup Success!"));
                    }
                    else
                    {
                        return Json(JsonBody.CreateResponse(false, "Signup Failed, Name is duplicated."));
                    }
                }
                return Json(JsonBody.CreateResponse(false, "Signup Failed, Passwords are not the same."));
            }
            return Json(JsonBody.CreateResponse(false, "Signup Failed, No Input."));
        }
        [HttpDelete]
        [Route("Logout")]
        public JsonResult Logout()
        {
            if (userService.Logoff(Convert.ToInt32(User.FindFirst(ClaimTypes.Sid)?.Value!)).IsCompleted)
            {
                HttpContext.Response.Cookies.Delete("Token");
                return Json(JsonBody.CreateResponse(true, "Logout success!"));
            }
            return Json(JsonBody.CreateResponse(false, "Logout failed!"));
        }
        [HttpPost]
        [Route("api/2fv")]
        public JsonResult TwoFactor()
        {
            string user = User.FindFirst(ClaimTypes.Name)!.Value;
            TwoFactorAuthenticator twoFactorAuthenticator = new TwoFactorAuthenticator();
            var TwoFactorSecretCode = "TwoFactorSecretCode";
            var accountSecretKey = $"{TwoFactorSecretCode}-{user}";
            var setupCode = twoFactorAuthenticator.GenerateSetupCode("BBS", user, Encoding.ASCII.GetBytes(accountSecretKey));
            var qrCodeUrl = setupCode.QrCodeSetupImageUrl;
            var manualCode = setupCode.ManualEntryKey;
            return Json(JsonBody.CreateResponse(true, new { qrCodeUrl, manualCode }, "Two Factor Authentication Setup Success"));
        }
        [HttpPost]
        [Route("apt/2fv/vali")]
        public JsonResult Validate([FromBody] JsonElement code)
        {
            var Authenticator = new TwoFactorAuthenticator();
            var valid = Authenticator.ValidateTwoFactorPIN($"TwoFactorSecretCode-{User.FindFirst(ClaimTypes.Name)!.Value}", code.GetString("code"));
            if (valid)
            {
                return Json(JsonBody.CreateResponse(true, "Two Factor Authentication Success"));
            }
            return Json(JsonBody.CreateResponse(false, "Two Factor Authentication Failed"));
        }
    }
}
