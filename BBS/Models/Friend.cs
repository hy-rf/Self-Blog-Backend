using System.ComponentModel.DataAnnotations.Schema;

namespace BBS.Models
{
    public class Friend
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        public int FriendUserId { get; set; }
        [ForeignKey("FriendUserId")]
        public User FriendUser { get; set; }
    }
}
