
using BBS.Data;
using BBS.Hubs;
using BBS.IService;

namespace BBS.Services
{
    public class NotificationService(AppDbContext ctx) : INotificationService
    {
        public async Task<Task> AddNotification(Models.Notification notification)
        {
            ctx.Notification.Add(notification);
            await ctx.SaveChangesAsync();
            return Task.CompletedTask;
        }
    }
}