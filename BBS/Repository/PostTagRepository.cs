using BBS.Data;
using BBS.IRepository;
using BBS.Models;

namespace BBS.Repository
{
    public class PostTagRepository(AppDbContext context) : BaseRepository<PostTag>(context), IPostTagRepository
    {
    }


}