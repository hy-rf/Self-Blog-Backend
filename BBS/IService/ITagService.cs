using BBS.Models;

namespace BBS.IService
{
    public interface ITagService
    {
        public List<PostTag> PostTags(int TagId);
        public void AddTag(string Name);
    }
}
