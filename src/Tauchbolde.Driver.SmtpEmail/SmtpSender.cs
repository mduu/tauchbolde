﻿using Microsoft.Extensions.Options;
using JetBrains.Annotations;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using Tauchbolde.Application.Services.Telemetry;

namespace Tauchbolde.Driver.SmtpEmail
{
    internal class SmtpSender : IAppEmailSender
    {
        private readonly SmtpSenderConfiguration options;
        private readonly ITelemetryService telemetryService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SmtpSender"/> class.
        /// </summary>
        /// <param name="options">SMTP Configuration options to use.</param>
        /// <param name="telemetryService">The telemetry service to use.</param>
        public SmtpSender(
            [NotNull] IOptions<SmtpSenderConfiguration> options,
            [NotNull] ITelemetryService telemetryService)
        {
            this.options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            this.telemetryService = telemetryService ?? throw new ArgumentNullException(nameof(telemetryService));
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
                TrackEvent("SMTPSENDER-SENT", msg);
            }
        }
        
        private void TrackEvent(string name, [NotNull] MimeMessage message)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
            if (message == null) throw new ArgumentNullException(nameof(message));

            telemetryService.TrackEvent(
                name,
                new Dictionary<string, string>
                {
                    { "To", message.To?.ToString() ?? "" },
                    { "From", message.From?.ToString() ?? "" },
                    { "Subject", message.Subject ?? "" },
                    { "HtmlBody", message.HtmlBody ?? "" },
                });
        }
    }
}
