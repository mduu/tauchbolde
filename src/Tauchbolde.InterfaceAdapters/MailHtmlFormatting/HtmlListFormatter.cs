using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;
using Tauchbolde.Application.Services.Notifications;
using Tauchbolde.Domain.Entities;
using Tauchbolde.Domain.Helpers;
using Tauchbolde.Driver.SmtpEmail;
using Tauchbolde.InterfaceAdapters.TextFormatting;

namespace Tauchbolde.InterfaceAdapters.MailHtmlFormatting
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
            AddContextUrl(htmlBuilder, notification);
            htmlBuilder.AppendLine("</div>");
            
            htmlBuilder.AppendLine("</li>");
            htmlBuilder.AppendLine();
        }

        private void AddContextUrl(StringBuilder htmlBuilder, Notification notification)
        {
            AddEventUrl(htmlBuilder, notification.EventId);
            AddLogbookEntryUrl(htmlBuilder, notification?.LogbookEntryId);
        }

        private void AddEventUrl(StringBuilder htmlBuilder, Guid? eventId)
        {
            if (eventId != null && eventId != Guid.Empty)
            {
                var eventUrl = urlGenerator.GenerateEventUrl(
                    smtpSenderConfiguration.Value.RootUrl,
                    eventId.Value);

                // ReSharper disable once StringLiteralTypo
                htmlBuilder.Append($"<p><a href='{eventUrl}'>Mehr...</a></p>");
            }
        }

        private void AddLogbookEntryUrl(StringBuilder htmlBuilder, Guid? logbookEntryId)
        {
            if (logbookEntryId != null && logbookEntryId != Guid.Empty)
            {
                var logbookEntryUrl = urlGenerator.GenerateLogbookEntryUrl(
                    smtpSenderConfiguration.Value.RootUrl,
                    logbookEntryId.Value);

                // ReSharper disable once StringLiteralTypo
                htmlBuilder.Append($"<p><a href='{logbookEntryUrl}'>Mehr...</a></p>");
            }
        }
    }
}