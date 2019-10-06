using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Tauchbolde.Driver.SmtpEmail;

namespace Tauchbolde.Web.Services
{
    // This class is used by the application to send Email and SMS
    // when you turn on two-factor authentication in ASP.NET Identity.
    // For more details see this link http://go.microsoft.com/fwlink/?LinkID=532713
    public class AuthMessageSender : IEmailSender
    {
        private readonly IAppEmailSender emailSender;

        public AuthMessageSender(
            IAppEmailSender emailSender)
        {
            this.emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var s = $"Tauchbolde Website: {subject}";
            await emailSender.SendAsync(email, email, s, message);
        }

        public Task SendSmsAsync(string number, string message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }
}
