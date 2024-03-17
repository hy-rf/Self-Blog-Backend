using Microsoft.AspNetCore.Mvc;

namespace BBS.Controllers
{
    public class Tag : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
