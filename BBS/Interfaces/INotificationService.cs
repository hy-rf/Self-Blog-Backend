
using BBS.Hubs;

namespace BBS.Interfaces
{
    public interface INotificationService
    {
        public Task<Task> AddNotification(Models.Notification notification);
    }
}