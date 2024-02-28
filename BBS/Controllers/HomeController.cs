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
            //System.Diagnostics.Debug.WriteLine(_db.SqLiteConnection().ConnectionString);
            return View();
        }
        
    }
    
}

