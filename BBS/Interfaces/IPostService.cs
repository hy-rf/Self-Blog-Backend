using BBS.Models;

namespace BBS.Interfaces
{
    public interface IPostService
    {
        public bool CreatePost(string Title, string Content, string Tag, int UserId, out int Id);
        public bool EditPost(int Id, string Title, string Content, string Tag);
        public Post GetPost(int id);
        public List<Post> GetPosts();
        public List<Post> GetPostsLite();
        public List<Post> GetPostsByPage(int PageIndex, int NumPostPerPage);
        public int CountPost();
    }
}
