using BBS.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace BBS.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [Route("Welcome")]
        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("Id") != null)
            {
                
                return RedirectToAction("UserCenter");
            }
            return View();
        }
        [Route("UserCenter")]
        public IActionResult UserCenter(){
            ViewBag.Id = HttpContext.Session.GetInt32("Id");
            ViewBag.UserInfo = _userService.GetUser(ViewBag.Id);
            return View("UserCenter");
        }
        public ActionResult Signup(string username, string password)
        {
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                if (_userService.Signup(username, password))
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
        public ActionResult Login(string username, string password)
        {
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                if (_userService.Login(username, password))
                {
                    // TODO : Implement add session
                    HttpContext.Session.SetInt32("Id", _userService.GetUserId());
                    HttpContext.Session.SetString("Name", username);
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }
        [Route("User/EditAvatar/{Id}")]
        public ActionResult EditAvatar(int Id, IFormFile avatar)
        {
            if (avatar.Length > 0)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    avatar.CopyTo(ms);
                    string s = Convert.ToBase64String(ms.ToArray());
                    if (_userService.EditAvatar(Id, s))
                    {
                        return RedirectToAction("Index");
                    }
                }
            }
            return RedirectToAction("Index");
        }
    }
}
