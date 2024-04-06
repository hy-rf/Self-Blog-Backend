using BBS.IService;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using MimeKit;

namespace BBS.Services
{
    public class MailService : IMailService
    {
        public Task<bool> SendMail()
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("FromName", "heyu880202@gmail.com"));
            message.To.Add(new MailboxAddress("", "ryusean9922@gmail.com"));
            message.Subject = "subject";
            message.Body = new TextPart("plain") { Text = "messageBody" };
            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                client.Authenticate("heyu880202@gmail.com", "mlsp kkol pixm afvk");
                client.Send(message);
                client.Disconnect(true);
            }
            return Task.FromResult(true);
        }
    }
}
