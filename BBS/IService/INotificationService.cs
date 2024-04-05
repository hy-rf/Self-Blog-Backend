
using BBS.Hubs;

namespace BBS.IService
{
    public interface INotificationService
    {
        public Task<Task> AddNotification(Models.Notification notification);
    }
}