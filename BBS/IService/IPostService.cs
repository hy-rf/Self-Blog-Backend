using BBS.Models;

namespace BBS.IService
{
    public interface IPostService
    {
        public Task<bool> CreatePost(string Title, string Content, string Tag, int UserId);
        public Task<bool> EditPost(int Id, string Title, string Content, string Tag);
        public Task<Post> GetPost(int id);
        public List<Post> GetPostsByPage(int PageIndex, int NumPostPerPage);
        public int CountPost();
    }
}
