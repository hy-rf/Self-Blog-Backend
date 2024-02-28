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
            return View();
        }
        public ActionResult Signup(string username, string password)
        {
            if (_userService.Signup(username, password))
            {
                return View("UserCenter");
            }
            else
            {
                return View("Index");
            }
        }
        public IActionResult Login(string username, string password)
        {
            if (_userService.Login(username, password))
            {
                // TODO : Implement add session
                return View("UserCenter");
            }
            else
            {
                return View("Index");
            }
        }
    }
}
