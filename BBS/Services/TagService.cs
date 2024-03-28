using BBS.Data;
using BBS.Interfaces;
using BBS.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace BBS.Services
{
    public class TagService(AppDbContext ctx) : ITagService
    {
        public List<PostTag> PostTags(int TagId)
        {
            return ctx.PostTag.Include(pt => pt.Post).ThenInclude(p => p.User).Where(pt => pt.TagId == TagId).ToList();
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
