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
        // public List<Tag>? Tags { get; set; }
    }
}
