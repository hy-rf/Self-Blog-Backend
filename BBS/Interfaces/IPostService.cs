using BBS.Models;

namespace BBS.Interfaces
{
    public interface IPostService
    {
        public bool CreatePost(string Title, string Content, int UserId);
        public bool EditPost(int Id, string Title, string Content);
        public object GetPost(int id);
        public List<Post> GetPosts();
    }
}
