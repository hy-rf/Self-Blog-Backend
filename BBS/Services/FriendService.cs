using BBS.Data;
using BBS.Interfaces;
using BBS.Models;

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
            throw new NotImplementedException();
        }

        public List<FriendRequest> FriendRequestsSent(int UserId)
        {
            throw new NotImplementedException();
        }

        public List<Friend> Friends(int UserId)
        {
            throw new NotImplementedException();
        }

        public void RemoveFriend(Friend friend, Friend friendOpposite)
        {
            throw new NotImplementedException();
        }

        public void RemoveFriendRequest(FriendRequest friendRequest)
        {

            throw new NotImplementedException();
        }
    }
}
