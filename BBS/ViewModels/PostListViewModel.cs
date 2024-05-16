using BBS.Models;

namespace BBS.ViewModels
{
    public class PostListViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ContentPreview { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public List<Tag> Tags { get; set; }
        public List<UserBriefViewModel> UsersWhoLike { get; set; }
    }
}
