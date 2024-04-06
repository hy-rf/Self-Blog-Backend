using BBS.IRepository;
using BBS.IService;

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