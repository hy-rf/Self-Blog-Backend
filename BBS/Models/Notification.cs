
namespace BBS.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public required string Type { get; set; }
        public required string Message { get; set; }
        public required string Url { get; set; }
        public bool IsRead { get; set; }
    }
}