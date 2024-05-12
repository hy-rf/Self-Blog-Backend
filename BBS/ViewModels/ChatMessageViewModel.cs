namespace BBS.ViewModels
{
    public class ChatMessageViewModel
    {
        public int Id { get; set; }
        public int ChatRoomId { get; set; }
        public int UserId { get; set; }
        public string Message { get; set; }
        public DateTime Created { get; set; }
        public string UserName { get; set; }
        public string UserAvatar { get; set; }
    }
}
