using Microsoft.AspNetCore.Mvc;

namespace BBS.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
    
}

