
using BBS.Hubs;

namespace BBS.IService
{
    public interface INotificationService
    {
        public Task<bool> AddNotification(Models.Notification notification);
    }
}