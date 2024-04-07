namespace BBS.IService
{
    public interface INotificationService
    {
        public Task<bool> AddNotification(Models.Notification notification);
        public Task<List<Models.Notification>> GetAllNotifications(int UserId);
    }
}