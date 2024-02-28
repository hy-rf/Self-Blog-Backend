using BBS.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BBS.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("Id") != null)
            {
                ViewBag.Id = HttpContext.Session.GetInt32("Id");
                ViewBag.UserInfo = _userService.GetUser(ViewBag.Id);
                return View("UserCenter");
            }
            return View();
        }
        public ActionResult Signup(string username, string password)
        {
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                if (_userService.Signup(username, password))
                {
                    return View("UserCenter");
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
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }
    }
}
