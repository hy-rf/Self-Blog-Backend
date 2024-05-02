using BBS.Data;
using BBS.IRepository;
using BBS.Models;

namespace BBS.Repository
{
    public class NotificationRepository(ForumContext context) : BaseRepository<Notification>(context), INotificationRepository
    {
    }


}