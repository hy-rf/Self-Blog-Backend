namespace BBS.Interfaces
{
    public interface ILikeService
    {
        void AddLike(int PostId, string UserId);
        void RemoveLike(int PostId, string UserId);
    }
}
