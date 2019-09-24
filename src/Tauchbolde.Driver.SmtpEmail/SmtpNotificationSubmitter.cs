using System;
using System.Threading.Tasks;
using Tauchbolde.Application.Services.Notifications;
using Tauchbolde.Domain.Entities;

namespace Tauchbolde.Driver.SmtpEmail
{
    internal class SmtpNotificationSubmitter : INotificationSubmitter
    {
        private readonly IAppEmailSender emailSender;

        public SmtpNotificationSubmitter(IAppEmailSender emailSender)
            => this.emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));

        public async Task SubmitAsync(Diver recipient, string content)
        {
            if (recipient == null) throw new ArgumentNullException(nameof(recipient));

            await emailSender.SendAsync(
                recipient.Fullname,
                recipient.User.Email,
                 "Tauchbolde Action-Log",
                content,
                "webmaster@tauchbolde.ch",
                "Tauchbolde Webmaster");
        }
    }
}