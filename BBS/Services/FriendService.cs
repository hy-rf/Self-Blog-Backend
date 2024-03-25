using BBS.Data;
using BBS.Interfaces;
using BBS.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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
            ctx.FriendRequest.Add(friendRequest);
            ctx.SaveChanges();
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

        public IEnumerable<object> Friends(int UserId)
        {
            var friends = ctx.Friend.Where(f => f.UserId == UserId);
            var ret = from f in ctx.Friend.Where(f => f.UserId == UserId)
                      join user in ctx.User on f.FriendUserId equals user.Id
                      select new
                      {
                          f.FriendUserId,
                          user.Id,
                          user.Name,
                          user.Created,
                          user.LastLogin,
                          user.Avatar
                      };
            return ret.ToList();
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
