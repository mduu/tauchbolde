using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;
using Tauchbolde.Common.Domain.SMTPSender;
using Tauchbolde.Common.Domain.TextFormatting;
using Tauchbolde.Common.Model;

namespace Tauchbolde.Common.Domain.Notifications.HtmlFormatting
{
    public class HtmlListFormatter : IHtmlListFormatter
    {
        [NotNull] private readonly IUrlGenerator urlGenerator;
        [NotNull] private readonly INotificationTypeInfos notificationTypeInfos;
        [NotNull] private readonly ITextFormatter textFormatter;
        [NotNull] private readonly IOptions<SmtpSenderConfiguration> smtpSenderConfiguration;

        public HtmlListFormatter(
            [NotNull] IUrlGenerator urlGenerator,
            [NotNull] INotificationTypeInfos notificationTypeInfos,
            [NotNull] ITextFormatter textFormatter,
            [NotNull] IOptions<SmtpSenderConfiguration> smtpSenderConfiguration)
        {
            this.urlGenerator = urlGenerator ?? throw new ArgumentNullException(nameof(urlGenerator));
            this.notificationTypeInfos = notificationTypeInfos ?? throw new ArgumentNullException(nameof(notificationTypeInfos));
            this.textFormatter = textFormatter ?? throw new ArgumentNullException(nameof(textFormatter));
            this.smtpSenderConfiguration = smtpSenderConfiguration ?? throw new ArgumentNullException(nameof(smtpSenderConfiguration));
        }

        public void Format(IEnumerable<Notification> notifications, StringBuilder htmlBuilder)
        {
            htmlBuilder.AppendLine("<ul class='list'>");
            foreach (var notification in notifications.OrderBy(n => n.OccuredAt))
            {
                FormatNotification(htmlBuilder, notification);
            }
            htmlBuilder.AppendLine("</ul>");
        }

        private void FormatNotification(StringBuilder htmlBuilder, Notification notification)
        {
            htmlBuilder.AppendLine($"<li class='{notification.Type.ToString()}'>");
            
            htmlBuilder.AppendLine("<div>");
            htmlBuilder.Append("<span class='timestamp'>");
            htmlBuilder.Append(notification.OccuredAt.ToLocalTime().ToStringSwissDateTime());
            htmlBuilder.Append("</span>");
            htmlBuilder.Append("<span class='message-type'>");
            htmlBuilder.Append(notificationTypeInfos.GetCaption(notification.Type));
            htmlBuilder.Append("</span>");
            htmlBuilder.AppendLine("</div>");

            htmlBuilder.AppendLine("<div class='message'>");
            htmlBuilder.Append(textFormatter.GetHtmlText(notification.Message));
            if (notification.EventId != Guid.Empty)
            {
                var eventUrl = urlGenerator.GenerateEventUrl(
                    smtpSenderConfiguration.Value.RootUrl,
                    notification.EventId);
                
                // ReSharper disable once StringLiteralTypo
                htmlBuilder.Append($" <a href='{eventUrl}'>Mehr...</a>");
            }
            htmlBuilder.AppendLine("</div>");
            
            htmlBuilder.AppendLine("</li>");
            htmlBuilder.AppendLine();
        }
    }
}