using BBS.Data;
using BBS.Models;
using Microsoft.AspNetCore.Mvc;

namespace BBS.Controllers
{
    public class HomeController(AppDbContext ctx) : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
        public ActionResult Search(string Option, string SearchTerm)
        {
            switch (Option)
            {
                case "Title":
                    List<Post> ret = ctx.Post.Where(p => p.Title!.Contains(SearchTerm)).ToList();
                    return View(ret);
                case "Content":
                    List<Post> ret2 = ctx.Post.Where(p => p.Content!.Contains(SearchTerm)).ToList();
                    return View(ret2);
                case "Reply":
                    List<Reply> ret3 = ctx.Reply.Where(r => r.Content!.Contains(SearchTerm)).ToList();
                    return View(ret3);
            }
            return View("Index");
        }

    }

}

