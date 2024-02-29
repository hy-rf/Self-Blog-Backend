namespace BBS.Models
{
    public class Post
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Content { get; set; }
        public required int UserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool Featured { get; set; }
        public bool Visibility { get; set; }
        public string? Tags { get; set; }
        public int? Likes { get; set; }
    }
}
