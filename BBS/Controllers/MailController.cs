
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using MimeKit;

namespace BBS.Controllers
{
    public class MailController : Controller
    {
        public IActionResult SendMail()
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
            return View();
        }
    }
}