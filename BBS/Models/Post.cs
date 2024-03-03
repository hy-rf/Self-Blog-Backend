namespace BBS.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool Featured { get; set; }
        public bool Visibility { get; set; }
        public string? Tags { get; set; }
    }
}
