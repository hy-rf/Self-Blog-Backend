
using System.Linq.Expressions;
using BBS.Data;
using BBS.IRepository;
using BBS.Models;
using Microsoft.EntityFrameworkCore;

namespace BBS.Repository
{
    public class PostRepository(AppDbContext context) : BaseRepository<Post>(context), IPostRepository
    {
        public Task<List<Post>> GetPostsForPostList(int PageIndex, int NumPostPerPage)
        {
            return Task.FromResult(context.Post.Include(p => p.User).Include(p => p.Likes).ThenInclude(l => l.User).Include(p => p.Replies).OrderByDescending(p => p.Id).Skip((PageIndex - 1) * NumPostPerPage).Take(NumPostPerPage).ToList());
        }
    }


}