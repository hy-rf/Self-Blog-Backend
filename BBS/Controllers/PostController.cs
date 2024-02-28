using Microsoft.AspNetCore.Mvc;

namespace BBS.Controllers
{
    public class PostController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
