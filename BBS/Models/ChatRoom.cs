using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BBS.Models
{
    public class ChatRoom
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        [NotMapped]
        public List<ChatRoomMessage>? ChatMessages { get; set; }
        [NotMapped]
        public List<ChatRoomMember>? ChatRoomMembers { get; set; }
    }
}
