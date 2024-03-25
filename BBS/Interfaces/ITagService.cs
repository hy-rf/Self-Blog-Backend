using BBS.Models;

namespace BBS.Interfaces
{
    public interface ITagService
    {
        public List<PostTag> PostTags(int TagId);
    }
}
