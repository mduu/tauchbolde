using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;
using Tauchbolde.Application.Services.Notifications;
using Tauchbolde.Domain.Entities;
using Tauchbolde.Driver.SmtpEmail;
using Tauchbolde.InterfaceAdapters.TextFormatting;
using Tauchbolde.SharedKernel.Extensions;

namespace Tauchbolde.InterfaceAdapters.MailHtmlFormatting
{
    public class HtmlListFormatter : IHtmlListFormatter
    {
        [NotNull] private readonly IAbsoluteUrlGenerator absoluteUrlGenerator;
        [NotNull] private readonly INotificationTypeInfos notificationTypeInfos;
        [NotNull] private readonly ITextFormatter textFormatter;
        [NotNull] private readonly IOptions<SmtpSenderConfiguration> smtpSenderConfiguration;

        public HtmlListFormatter(
            [NotNull] IAbsoluteUrlGenerator absoluteUrlGenerator,
            [NotNull] INotificationTypeInfos notificationTypeInfos,
            [NotNull] ITextFormatter textFormatter,
            [NotNull] IOptions<SmtpSenderConfiguration> smtpSenderConfiguration)
        {
            this.absoluteUrlGenerator = absoluteUrlGenerator ?? throw new ArgumentNullException(nameof(absoluteUrlGenerator));
            this.notificationTypeInfos = notificationTypeInfos ?? throw new ArgumentNullException(nameof(notificationTypeInfos));
            this.textFormatter = textFormatter ?? throw new ArgumentNullException(nameof(textFormatter));
            this.smtpSenderConfiguration = smtpSenderConfiguration ?? throw new ArgumentNullException(nameof(smtpSenderConfiguration));
        }

        public void Format(IEnumerable<Notification> notifications, StringBuilder htmlBuilder)
        {
            htmlBuilder.AppendLine("<table class='notification-list' CELLPADDING=\"0\" CELLSPACING=\"0\">");
            foreach (var notification in notifications.OrderBy(n => n.OccuredAt))
            {
                FormatNotification(htmlBuilder, notification);
            }

            htmlBuilder.AppendLine("</table>");
        }

        private void FormatNotification(StringBuilder htmlBuilder, Notification notification)
        {
            htmlBuilder.AppendLine("<tr>");

            htmlBuilder.AppendLine("<td class=\"cell-icon\" width=\"55\">");
            
            var iconData = notificationTypeInfos.GetIconBase64(notification.Type);
            if (!string.IsNullOrWhiteSpace(iconData))
            {
                var htmlImageSource = $"data:image/png;base64, {iconData}";
                htmlBuilder.AppendLine($"<img class=\"icon\" src=\"{htmlImageSource}\" />");
            }

            htmlBuilder.AppendLine("</td>");

            htmlBuilder.AppendLine("<td class=\"cell-message\" width=\"100%\">");
            
            htmlBuilder.AppendLine($"<div class=\"notification-item {notification.Type.ToString()}\">");

            htmlBuilder.Append("<span class=\"timestamp\">");
            htmlBuilder.Append(notification.OccuredAt.ToLocalTime().ToStringSwissDateTime());
            htmlBuilder.Append("</span>");
            htmlBuilder.Append("<span class='message-type'>");
            htmlBuilder.Append($"{notificationTypeInfos.GetCaption(notification.Type)}&nbsp;");
            htmlBuilder.Append("</span>");

            htmlBuilder.AppendLine("<p class='message'>");
            htmlBuilder.Append(textFormatter.GetHtmlText(notification.Message));
            AddContextUrl(htmlBuilder, notification);
            htmlBuilder.AppendLine("</p>");

            htmlBuilder.AppendLine("</div>");
            
            htmlBuilder.AppendLine("</td>");
            
            htmlBuilder.AppendLine("</tr>");
        }

        private void AddContextUrl(StringBuilder htmlBuilder, Notification notification)
        {
            AddEventUrl(htmlBuilder, notification.EventId);
            AddLogbookEntryUrl(htmlBuilder, notification.LogbookEntryId);
        }

        private void AddEventUrl(StringBuilder htmlBuilder, Guid? eventId)
        {
            if (eventId != null && eventId != Guid.Empty)
            {
                var eventUrl = absoluteUrlGenerator.GenerateEventUrl(
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
                var logbookEntryUrl = absoluteUrlGenerator.GenerateLogbookEntryUrl(
                    smtpSenderConfiguration.Value.RootUrl,
                    logbookEntryId.Value);

                // ReSharper disable once StringLiteralTypo
                htmlBuilder.Append($"<p><a href='{logbookEntryUrl}'>Mehr...</a></p>");
            }
        }
    }
}