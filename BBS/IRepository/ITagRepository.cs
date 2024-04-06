using BBS.Models;

namespace BBS.IRepository
{
    public interface ITagRepository : IBaseRepository<Tag>
    {
        public Task<Tag> GetTagByNameAsync(string name);
    }

    
}