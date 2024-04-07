using System.ComponentModel.DataAnnotations.Schema;

namespace BBS.Models
{
    public class Reply
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PostId { get; set; }
        public string? Content { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        [ForeignKey("UserId")]
        public User? User { get; set; }
        public List<LikedReply>? Likes { get; set; }
    }
}
