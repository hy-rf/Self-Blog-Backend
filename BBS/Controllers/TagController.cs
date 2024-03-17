using Microsoft.AspNetCore.Mvc;

namespace BBS.Controllers
{
    public class TagController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
