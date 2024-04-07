
using BBS.Models;

namespace BBS.IRepository
{
    public interface IPostRepository : IBaseRepository<Post>
    {
        Task<int> CountPost();
        Task<List<Post>> GetPostsForPostList(int PageIndex, int NumPostPerPage);
        Task<Post> GetPostById(int Id);
    }


}