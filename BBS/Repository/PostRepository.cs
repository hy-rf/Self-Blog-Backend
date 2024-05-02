using BBS.Data;
using BBS.IRepository;
using BBS.Models;
using Microsoft.EntityFrameworkCore;

namespace BBS.Repository
{
    public class PostRepository(ForumContext context) : BaseRepository<Post>(context), IPostRepository
    {
        public Task<int> CountPost()
        {
            return Task.FromResult(context.Post.Count());
        }

        public Task<Post> GetPostById(int Id)
        {
            return Task.FromResult(context.Post.Include(p => p.PostTags)!.ThenInclude(pt => pt.Tag).Include(p => p.Replies)!.ThenInclude(r => r.User).Include(p => p.User).Include(p => p.Likes).ThenInclude(l => l.User).Single(p => p.Id == Id));
        }

        public Task<List<Post>> GetPostsForPostList(int PageIndex, int NumPostPerPage)
        {
            return Task.FromResult(context.Post.Include(p => p.User).Include(p => p.Likes).ThenInclude(l => l.User).Include(p => p.Replies).OrderByDescending(p => p.Id).Skip((PageIndex - 1) * NumPostPerPage).Take(NumPostPerPage).ToList());
        }
    }


}