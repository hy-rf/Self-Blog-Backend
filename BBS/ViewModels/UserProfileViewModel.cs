using BBS.Models;

namespace BBS.ViewModels
{
    public class UserProfileViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastLogin { get; set; }
        public string Avatar { get; set; }
        public List<Post> Posts { get; set; }
        public List<Reply> Replies { get; set; }
    }
}
