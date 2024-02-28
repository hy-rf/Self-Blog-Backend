namespace BBS.Models
{
    public class Reply
    {
        public int Id { get; set; }
        public required string Content { get; set; }
        public int UserId { get; set; }
        public required string UserName { get; set; }
        public int PostId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool Visibility { get; set; }
    }
}
