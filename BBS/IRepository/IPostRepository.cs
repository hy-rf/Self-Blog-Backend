
using BBS.Models;

namespace BBS.IRepository
{
    public interface IPostRepository : IBaseRepository<Post>
    {
        Task<List<Post>> GetPostsForPostList(int PageIndex, int NumPostPerPage);
    }


}