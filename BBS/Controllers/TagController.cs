using BBS.Data;
using BBS.Interfaces;
using BBS.Models;
using Microsoft.AspNetCore.Mvc;

namespace BBS.Controllers
{
    public class TagController(AppDbContext ctx) : Controller
    {
        public IActionResult Index(string tag)
        {
            var postids = ctx.Tag.Select(t => new { t.Name, t.PostId }).Where(t => t.Name == tag);
            List<Post> posts = new List<Post>();
            foreach (var id in postids)
            {
                posts.Add(ctx.Post.Single(p => p.Id == id.PostId));
            }
            //select posts where id = postids.PostId

            return View(posts);
        }
    }
}
