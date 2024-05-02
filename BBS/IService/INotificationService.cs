namespace BBS.IService
{
    public interface INotificationService
    {
        public Task AddNotification(Models.Notification notification);
        public Task<List<Models.Notification>> GetAllNotifications(int UserId);
    }
}