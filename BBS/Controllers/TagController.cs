using BBS.Data;
using BBS.Interfaces;
using BBS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BBS.Controllers
{
    public class TagController(AppDbContext ctx) : Controller
    {
        public IActionResult Index(int Id, string tag)
        {
            var posts = ctx.PostTag.Include(pt => pt.Post).ThenInclude(p => p.User).Where(pt => pt.TagId == Id).ToList();
            return View(posts);
        }
    }
}
