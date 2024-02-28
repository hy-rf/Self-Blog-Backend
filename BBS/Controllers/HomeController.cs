using Microsoft.AspNetCore.Mvc;
using BBS.Interfaces;

namespace BBS.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDatabase _db;
        public HomeController(IDatabase db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }
        
    }
    
}

