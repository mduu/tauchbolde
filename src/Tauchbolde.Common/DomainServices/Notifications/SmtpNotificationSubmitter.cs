using System;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using Tauchbolde.Common.Model;

namespace Tauchbolde.Common.DomainServices.Notifications
{
    public class SmtpNotificationSubmitter : INotificationSubmitter
    {
        public async Task SubmitAsync(UserInfo recipient, string content)
        {
            if (recipient == null) throw new ArgumentNullException(nameof(recipient));

            var msg = new MimeMessage();
            msg.From.Add(new MailboxAddress("Tauchbolde Webmaster", "webmaster@tauchbolde.ch"));
            msg.Subject = "Tauchbolde Action-Log";
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