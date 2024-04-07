using BBS.Models;

namespace BBS.IService
{
    public interface IFriendService
    {
        public void AddFriendRequest(FriendRequest friendRequest);
        public void RemoveFriendRequest(FriendRequest friendRequest);
        public List<FriendRequest> FriendRequests(int UserId);
        public List<FriendRequest> FriendRequestsSent(int UserId);
        public void AddFriend(Friend friend, Friend friendOpposite);
        public void RemoveFriend(Friend friend, Friend friendOpposite);
        public IAsyncEnumerable<Friend> Friends(int UserId);
        public bool isFriend(int UserId, int FriendUserId);
        public bool isFriendRequestSent(int UserId, int FriendUserId);

    }
}
