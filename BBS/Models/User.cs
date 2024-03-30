using System.ComponentModel.DataAnnotations.Schema;

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
        public int LoggedIn {  get; set; }
        public ICollection<Post>? Posts { get; set; }
        public ICollection<Reply>? Replies { get; set; }
        //[NotMapped]
        //public ICollection<Friend>? Friends { get; set; }

    }
}
