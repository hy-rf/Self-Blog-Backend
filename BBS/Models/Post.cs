using System.ComponentModel.DataAnnotations.Schema;

namespace BBS.Models
{
    public class Post
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public List<Reply>? Replies { get; set; }
        [ForeignKey("PostId")]
        public List<PostTag>? PostTags { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        public List<LikedPost> Likes { get; set; }
    }
}
