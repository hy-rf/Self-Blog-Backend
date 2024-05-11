using BBS.IRepository;
using BBS.IService;
using BBS.Models;

namespace BBS.Services
{
    public class NotificationService(INotificationRepository notificationRepository) : INotificationService
    {
        public async Task AddNotification(Notification notification)
        {
            await notificationRepository.CreateAsync(notification);
        }

        public async Task<List<Notification>> GetAllNotifications(int UserId)
        {
            return await notificationRepository.GetAllAsync(n => n.UserId == UserId);
        }
    }
}