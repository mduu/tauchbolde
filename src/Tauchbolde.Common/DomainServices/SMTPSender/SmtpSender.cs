using MimeKit;
using MailKit.Net.Smtp;
using MimeKit.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MailKit.Security;
using System;

namespace Tauchbolde.Common.DomainServices.SMTPSender
{
    public class SmtpSender : IAppEmailSender
    {
        private readonly SmtpSenderConfiguration options;

        /// <summary>
        /// Initializes a new instance of the <see cref="SmtpSender"/> class.
        /// </summary>
        /// <param name="options">SMTP Configuration options to use.</param>
        public SmtpSender(IOptions<SmtpSenderConfiguration> options)
        {
            this.options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        /// <inheritdoc/>
        public async Task SendAsync(
            string receiverName,
            string receiverEmail,
            string subject,
            string content,
            string senderEmail = null,
            string senderName = null)
        {
            var msg = new MimeMessage();
            msg.To.Add(new MailboxAddress(receiverName, receiverEmail));
            
            msg.From.Add(
                new MailboxAddress(
                    senderName ?? options.SystemSenderName,
                    senderEmail ?? options.SystemSenderEmailAddress));
                    
            msg.Subject = subject;
            msg.Body = new TextPart(TextFormat.Html)
            {
                Text = content
            };

            var ssl = Enum.Parse<SecureSocketOptions>(options.Ssl);

            using (var smtp = new SmtpClient())
            {
                await smtp.ConnectAsync(options.Host, options.Port, ssl);
                if (!string.IsNullOrWhiteSpace(options.AuthUsername))
                {
                    await smtp.AuthenticateAsync(options.AuthUsername, options.AuthPassword);
                }
                await smtp.SendAsync(msg);
            }
        }
    }
}
