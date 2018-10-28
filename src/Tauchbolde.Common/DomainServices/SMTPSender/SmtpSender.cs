using MimeKit;
using MailKit.Net.Smtp;
using MimeKit.Text;
using System.Threading.Tasks;

namespace Tauchbolde.Common.DomainServices.SMTPSender
{
    public class SmtpSender : IAppEmailSender
    {
        public async Task Send(string receiverName, string receiverEmail, string senderName, string senderEmail, string subject, string content)
        {
            var msg = new MimeMessage();
            msg.To.Add(new MailboxAddress(receiverName, receiverEmail));
            msg.From.Add(new MailboxAddress(senderName, senderEmail));
            msg.Subject = subject;
            msg.Body = new TextPart(TextFormat.Html)
            {
                Text = content
            };

            using (var smtp = new SmtpClient())
            {
                await smtp.ConnectAsync("host", 587, false);
                await smtp.SendAsync(msg);
            }
        }
    }
}
