
using System.Linq.Expressions;
using BBS.Data;
using BBS.IRepository;
using BBS.Models;

namespace BBS.Repository
{
    public class NotificationRepository(AppDbContext context) : BaseRepository<Notification>(context), INotificationRepository
    {
    }


}