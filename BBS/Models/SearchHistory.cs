namespace BBS.Models
{
    public class SearchHistory
    {
        public int Id { get; set; }
        public string SearchTerm { get; set; }
        public string SearchOption { get; set; }
        public DateTime SearchDate { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
