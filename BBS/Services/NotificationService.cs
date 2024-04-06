
using BBS.Data;
using BBS.Hubs;
using BBS.IRepository;
using BBS.IService;
using BBS.Repository;

namespace BBS.Services
{
    public class NotificationService(INotificationRepository notificationRepository) : INotificationService
    {
        public async Task<bool> AddNotification(Models.Notification notification)
        {
            return await notificationRepository.CreateAsync(notification);
        }
    }
}