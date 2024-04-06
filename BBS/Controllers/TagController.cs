using BBS.IService;
using Microsoft.AspNetCore.Mvc;

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
