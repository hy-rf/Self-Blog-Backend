namespace BBS.IService
{
    public interface IMailService
    {
        public Task<bool> SendMail(string receiver, string subject, string body);
    }
}
