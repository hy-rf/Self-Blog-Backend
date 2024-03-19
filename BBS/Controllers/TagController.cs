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
            return View(ctx.PostTag.Where(pt => pt.TagId == Id).Include(pt => pt.Post));
        }
    }
}
