using System;
using System.Collections.Generic;
using System.Text;
using Tauchbolde.Common.Model;

namespace Tauchbolde.Common.DomainServices.Notifications.HtmlFormatting
{
    public class HtmlListFormatter : IHtmlListFormatter
    {
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

        private const string TheRedColor = "#cc0000";
        private const string TheGreenColor = "#006600";
        private const string TheBlueColor = "#003399";
        
        private string NotificationTypeToString(NotificationType notificationType)
        {
            switch (notificationType)
            {
                case NotificationType.NewEvent:
                    return GenerateColorText("Neue Aktivität", TheGreenColor);
                case NotificationType.CancelEvent:
                    return GenerateColorText("Aktivität abgesagt", TheRedColor);
                case NotificationType.EditEvent:
                    return GenerateColorText("Aktivität geändert", TheBlueColor);
                case NotificationType.Commented:
                    return GenerateColorText("Neuer Kommentar", TheGreenColor);
                case NotificationType.Accepted:
                    return GenerateColorText("Zusage", TheGreenColor);
                case NotificationType.Declined:
                    return GenerateColorText("Absage", TheRedColor);
                case NotificationType.Tentative:
                    return GenerateColorText("Vorbehalt", TheBlueColor);
                case NotificationType.Neutral:
                    return GenerateColorText("Unklar", TheBlueColor);
                default:
                    throw new ArgumentOutOfRangeException(nameof(notificationType));
            }
        }

        private static string GenerateColorText(string text, string color) =>
            $"<span style='color: {color}'>{text}</span>";

    }
}