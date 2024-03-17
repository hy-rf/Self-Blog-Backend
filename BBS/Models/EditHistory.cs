namespace BBS.Models
{
    public class EditHistory
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string UserId { get; set; }
        public DateTime EditedAt { get; set; }
    }
}
