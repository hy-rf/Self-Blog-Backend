namespace BBS.IService
{
    public interface IMailService
    {
        public Task<bool> SendMail();
    }
}
