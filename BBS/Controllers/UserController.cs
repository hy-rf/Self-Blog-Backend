﻿using BBS.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace BBS.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IPostService _postService;
        public UserController(IUserService userService, IPostService postService)
        {
            _userService = userService;
            _postService = postService;
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
        public IActionResult UserCenter()
        {
            try
            {
                ViewBag.Id = HttpContext.Session.GetInt32("Id");
                ViewBag.UserInfo = _userService.GetUser(ViewBag.Id);
                return View("UserCenter");
            }
            catch
            {
                return Unauthorized();
            }


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
                    HttpContext.Session.SetInt32("Id", Id);
                    HttpContext.Session.SetString("Name", Name);
                    return RedirectToAction("Index");
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
                HttpContext.Session.SetString("Name", name);
                Response.StatusCode = 200;
                return;
            }
            Response.StatusCode = 404;
            return;
        }
        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}
