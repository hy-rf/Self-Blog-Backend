using System.ComponentModel.DataAnnotations.Schema;

namespace BBS.Models
{
    public class FriendRequest
    {
        public int Id { get; set; }
        public int SendUserId { get; set; }
        public int ReceiveUserId { get; set; }
        [ForeignKey("SendUserId")]
        public User? SendUser { get; set; }
        [ForeignKey("ReceiveUserId")]
        public User? ReceiveUser { get; set; }
    }
}
