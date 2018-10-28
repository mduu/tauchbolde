using System;
using System.Threading.Tasks;
using Tauchbolde.Common.DomainServices.SMTPSender;
using Tauchbolde.Common.Model;

namespace Tauchbolde.Common.DomainServices.Notifications
{
    public class SmtpNotificationSubmitter : INotificationSubmitter
    {
        private readonly IAppEmailSender emailSender;

        public SmtpNotificationSubmitter(IAppEmailSender emailSender)
            => 
                this.emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));

        public async Task SubmitAsync(Diver recipient, string content)
        {
            if (recipient == null) throw new ArgumentNullException(nameof(recipient));

            await emailSender.Send(
                recipient.Fullname,
                recipient.User.Email,
                "Tauchbolde Webmaster",
                "webmaster@tauchbolde.ch",
                "Tauchbolde Action-Log",
                content
            );
        }
    }
}