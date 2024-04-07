using BBS.Models;

namespace BBS.IService
{
    public interface ILikeService
    {
        public void AddLikePost(LikedPost likedPost);
        public void RemoveLikePost(LikedPost likedPost);
        public void AddLikeReply(LikedReply likedReply);
        public void RemoveLikeReply(LikedReply likedReply);
        public List<LikedPost> likedPosts(int userId);
        public List<LikedReply> likedReplies(int userId);
    }
}
