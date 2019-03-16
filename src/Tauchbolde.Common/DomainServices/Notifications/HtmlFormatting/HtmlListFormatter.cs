using System;
using System.Collections.Generic;
using System.Text;
using Tauchbolde.Common.Model;

namespace Tauchbolde.Common.DomainServices.Notifications.HtmlFormatting
{
    public class HtmlListFormatter : IHtmlListFormatter
    {
        private const string TheRedColor = "#cc0000";
        private const string TheGreenColor = "#006600";
        private const string TheBlueColor = "#003399";
        
        private static readonly IDictionary<NotificationType, NotificationInfo> NotificationTypeInfos = 
            new Dictionary<NotificationType, NotificationInfo> {
                { NotificationType.NewEvent, new NotificationInfo { Color = TheGreenColor, Caption = "Neue Aktivität" }},
                { NotificationType.CancelEvent, new NotificationInfo { Color = TheRedColor, Caption = "Aktivität abgesagt" }},
                { NotificationType.EditEvent, new NotificationInfo { Color = TheBlueColor, Caption = "Aktivität geändert" }},
                { NotificationType.Commented, new NotificationInfo { Color = TheGreenColor, Caption = "Neuer Kommentar" }},
                { NotificationType.Accepted, new NotificationInfo { Color = TheGreenColor, Caption = "Zusage" }},
                { NotificationType.Declined, new NotificationInfo { Color = TheRedColor, Caption = "Absage" }},
                { NotificationType.Tentative, new NotificationInfo { Color = TheBlueColor, Caption = "Vorbehalt" }},
                { NotificationType.Neutral, new NotificationInfo { Color = TheBlueColor, Caption = "Unklar" }},
            };

        private readonly IUrlGenerator urlGenerator;

        public HtmlListFormatter(IUrlGenerator urlGenerator)
        {
            this.urlGenerator = urlGenerator ?? throw new ArgumentNullException(nameof(urlGenerator));
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

        private static string NotificationTypeToString(NotificationType notificationType)
        {
            var notificationInfo = NotificationTypeInfos[notificationType];
            if (notificationInfo == null)
            {
                throw new InvalidOperationException($"No mapping for notificationType [{notificationType.ToString()}] !");
            }
            
            return $"<span style='color: {notificationInfo.Color}'>{notificationInfo.Caption}</span>";
        }

        private class NotificationInfo
        {
            public string Caption { get; set; }
            public string Color { get; set; }
        }
    }
}