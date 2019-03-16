using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;
using Tauchbolde.Common.Model;

namespace Tauchbolde.Common.DomainServices.Notifications.HtmlFormatting
{
    public class HtmlListFormatter : IHtmlListFormatter
    {
        [NotNull] private readonly IUrlGenerator urlGenerator;
        [NotNull] private readonly INotificationTypeInfos notificationTypeInfos;

        public HtmlListFormatter(
            [NotNull] IUrlGenerator urlGenerator,
            [NotNull] INotificationTypeInfos notificationTypeInfos)
        {
            this.urlGenerator = urlGenerator ?? throw new ArgumentNullException(nameof(urlGenerator));
            this.notificationTypeInfos = notificationTypeInfos ?? throw new ArgumentNullException(nameof(notificationTypeInfos));
        }

        public void Format(IEnumerable<Notification> notifications, StringBuilder htmlBuilder)
        {
            htmlBuilder.AppendLine("<ul>");
            foreach (var notification in notifications)
            {
                FormatNotification(htmlBuilder, notification);
            }
            htmlBuilder.AppendLine("</ul>");
        }

        private void FormatNotification(StringBuilder htmlBuilder, Notification notification)
        {
            htmlBuilder.AppendLine("<li>");

            htmlBuilder.Append("<span style='font-size: small; color: grey;'>");
            htmlBuilder.Append(notification.OccuredAt.ToString("dd.MM.yyyy HH.mm "));
            htmlBuilder.Append("</span>");
            htmlBuilder.Append(NotificationTypeToString(notification.Type));
            htmlBuilder.Append(": ");
            htmlBuilder.Append(notification.Message);

            if (notification.EventId != Guid.Empty)
            {
                var eventUrl = urlGenerator.GenerateEventUrl(notification.EventId);
                htmlBuilder.Append($" <a href='{eventUrl}'>Mehr...</a>");
            }

            htmlBuilder.AppendLine();

            htmlBuilder.AppendLine("</li>");
        }

        private string NotificationTypeToString(NotificationType notificationType)
        {
            var notificationInfo = notificationTypeInfos.GetInfo(notificationType);
            if (notificationInfo == null)
            {
                throw new InvalidOperationException($"No mapping for notificationType [{notificationType.ToString()}] !");
            }
            
            return $"<span style='color: {notificationInfo.Color}'>{notificationInfo.Caption}</span>";
        }

    }
}