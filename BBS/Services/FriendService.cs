using BBS.Data;
using BBS.IService;
using BBS.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Linq;

namespace BBS.Services
{
    public class FriendService(AppDbContext ctx) : IFriendService
    {
        public void AddFriend(Friend friend, Friend friendOpposite)
        {
            ctx.Friend.Add(friend);
            ctx.Friend.Add(friendOpposite);
            ctx.SaveChanges();
        }

        public void AddFriendRequest(FriendRequest friendRequest)
        {
            if (!ctx.FriendRequest.Any(fr => fr.SendUserId == friendRequest.SendUserId && fr.ReceiveUserId == friendRequest.ReceiveUserId))
            {
                ctx.FriendRequest.Add(friendRequest);
                ctx.SaveChanges();
            }
        }

        public List<FriendRequest> FriendRequests(int UserId)
        {
            List<FriendRequest> ret = ctx.FriendRequest.Where(fr => fr.ReceiveUserId == UserId).Include(fr => fr.SendUser).ToList();
            return ret;
        }

        public List<FriendRequest> FriendRequestsSent(int UserId)
        {
            List<FriendRequest> ret = ctx.FriendRequest.Where(fr => fr.SendUserId == UserId).Include(fr => fr.ReceiveUser).ToList();
            return ret;
        }

        public IAsyncEnumerable<Friend> Friends(int UserId)
        {
            return (IAsyncEnumerable<Friend>)ctx.Friend.Where(f => f.UserId == UserId).Include(f => f.FriendUser).Select(f => new Friend
            {
                FriendUser = new User
                {
                    Id = f.FriendUser.Id,
                    Name = f.FriendUser.Name,
                    Created = f.FriendUser.Created,
                    Avatar = f.FriendUser.Avatar
                }
            });
        }

        public bool isFriend(int UserId, int FriendUserId)
        {
            return ctx.Friend.Any(f => f.UserId == UserId && f.FriendUserId == FriendUserId);
        }

        public bool isFriendRequestSent(int UserId, int FriendUserId)
        {
            return ctx.FriendRequest.Any(fq => fq.SendUserId == UserId && fq.ReceiveUserId == FriendUserId);
        }

        public void RemoveFriend(Friend friend, Friend friendOpposite)
        {
            ctx.Friend.Remove(friend);
            ctx.Friend.Remove(friendOpposite);
            ctx.SaveChanges();
        }

        public void RemoveFriendRequest(FriendRequest friendRequest)
        {
            ctx.FriendRequest.Remove(ctx.FriendRequest.Where(fr => fr.ReceiveUserId == friendRequest.ReceiveUserId && fr.SendUserId == friendRequest.SendUserId).Single());
            ctx.SaveChanges();
        }
    }
}
