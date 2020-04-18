using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Tauchbolde.Application.Services.Telemetry;
using Microsoft.AspNetCore.Identity.UI.Services;
using Tauchbolde.Driver.SmtpEmail;

namespace Tauchbolde.Web.Services
{
    // This class is used by the application to send Email and SMS
    // when you turn on two-factor authentication in ASP.NET Identity.
    // For more details see this link http://go.microsoft.com/fwlink/?LinkID=532713
    public class IdentityMessageSender : IEmailSender
    {
        [NotNull] private readonly IAppEmailSender emailSender;
        [NotNull] private readonly ILogger logger;
        [NotNull] private readonly ITelemetryService telemetryService;

        public IdentityMessageSender(
            [NotNull] IAppEmailSender emailSender,
            [NotNull] ILogger<IdentityMessageSender> logger,
            [NotNull] ITelemetryService telemetryService)
        {
            this.emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.telemetryService = telemetryService ?? throw new ArgumentNullException(nameof(telemetryService));
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var s = $"Tauchbolde Website: {subject}";
            
            telemetryService.TrackEvent(
                TelemetryEventNames.IdentityMailSent,
                new { address = email, subject});
            logger.LogWarning("Sending auth. email: Address={email};Subject={s};Message={message}");
            
            await emailSender.SendAsync(email, email, s, message);
        }

        public Task SendSmsAsync(string number, string message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }
}
