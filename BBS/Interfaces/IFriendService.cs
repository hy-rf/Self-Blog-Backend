using BBS.Models;
using Microsoft.AspNetCore.Mvc;

namespace BBS.Interfaces
{
    public interface IFriendService
    {
        public void AddFriendRequest(FriendRequest friendRequest);
        public void RemoveFriendRequest(FriendRequest friendRequest);
        public List<FriendRequest> FriendRequests(int UserId);
        public List<FriendRequest> FriendRequestsSent(int UserId);
        public void AddFriend(Friend friend, Friend friendOpposite);
        public void RemoveFriend(Friend friend, Friend friendOpposite);
        public IEnumerable<object> Friends(int UserId);


    }
}
