namespace BBS.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Pwd { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastLogin { get; set; }
        public string? Avatar { get; set; }
        public IEnumerable<Post>? Posts { get; set; }
        public IEnumerable<Reply>? Replies { get; set; }

    }
}
