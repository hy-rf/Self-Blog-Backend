
using System.Linq.Expressions;
using BBS.IRepository;
using BBS.Models;

namespace BBS.Repository
{
    public class PostRepository : IPostReopsitory
    {
        public Task<bool> CreateAsync(Post entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Post>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<Post>> GetAllAsync(Expression<Func<Post, bool>> whereLambda)
        {
            throw new NotImplementedException();
        }

        public Task<List<Post>> GetAllAsync(int page, int size, Expression<Func<Post, bool>> whereLambda)
        {
            throw new NotImplementedException();
        }

        public async Task<Post> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(Post entity)
        {
            throw new NotImplementedException();
        }
    }


}