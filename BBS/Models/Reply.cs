namespace BBS.Models
{
    public class Reply
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int PostId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool Visibility { get; set; }
        public string? Likes { get; set; }
    }
}
