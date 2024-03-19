namespace BBS.Models
{
    public class ViewHistory
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string UserId { get; set; }
        public DateTime ViewedAt { get; set; }
    }
}
