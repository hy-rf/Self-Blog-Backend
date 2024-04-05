using BBS.Data;
using BBS.IService;
using BBS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BBS.Controllers
{
    public class TagController(ITagService tagService) : Controller
    {
        [Route("Tag/Index/{Id}")]
        public IActionResult Index(int Id)
        {
            return View(tagService.PostTags(Id));
        }
    }
}
