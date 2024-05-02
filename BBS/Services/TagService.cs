using BBS.Data;
using BBS.IService;
using BBS.Models;
using Microsoft.EntityFrameworkCore;

namespace BBS.Services
{
    public class TagService(ForumContext ctx) : ITagService
    {
        public List<PostTag> PostTags(int TagId)
        {
            return ctx.PostTag.Include(pt => pt.Post).ThenInclude(p => p.User).Include(pt => pt.Tag).Where(pt => pt.TagId == TagId).ToList();
        }
        public void AddTag(string Name)
        {
            var newtag = new Tag
            {
                Name = Name
            };
            ctx.Tag.Add(newtag);
            ctx.SaveChanges();
        }
    }
}
