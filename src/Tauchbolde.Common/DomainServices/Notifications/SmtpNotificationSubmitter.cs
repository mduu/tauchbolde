using System;
using System.Threading.Tasks;
using Tauchbolde.Common.DomainServices.SMTPSender;
using Tauchbolde.Common.Model;

namespace Tauchbolde.Common.DomainServices.Notifications
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