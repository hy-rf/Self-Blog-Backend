using BBS.Models;

namespace BBS.IRepository
{
    public interface IPostTagRepository : IBaseRepository<PostTag>
    {
        public Task<List<PostTag>> GetPostTagsByPostId(int PostId);
    }


}