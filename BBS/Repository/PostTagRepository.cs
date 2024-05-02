using BBS.Data;
using BBS.IRepository;
using BBS.Models;
using Microsoft.EntityFrameworkCore;

namespace BBS.Repository
{
    public class PostTagRepository(ForumContext context) : BaseRepository<PostTag>(context), IPostTagRepository
    {
        public async Task<List<PostTag>> GetPostTagsByPostId(int PostId)
        {
            return await Task.FromResult(context.PostTag.Where(t => t.PostId == PostId).Include(t => t.Tag).ToList());
        }
    }


}