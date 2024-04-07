using BBS.Data;
using BBS.IRepository;
using BBS.Models;
using Microsoft.EntityFrameworkCore;

namespace BBS.Repository
{
    public class PostTagRepository(AppDbContext context) : BaseRepository<PostTag>(context), IPostTagRepository
    {
        public Task<List<PostTag>> GetPostTagsByPostId(int PostId)
        {
            return Task.FromResult(context.PostTag.Where(t => t.PostId == PostId).Include(t => t.Tag).ToList());
        }
    }


}