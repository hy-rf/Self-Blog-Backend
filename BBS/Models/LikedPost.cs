namespace BBS.Models
{
    public class LikedPost
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }
    }
}
