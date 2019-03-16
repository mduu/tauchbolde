using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tauchbolde.Common.Model;

namespace Tauchbolde.Common.DomainServices.Notifications
{
    /// <summary>
    /// Formats notification for a given recipient as HTML.
    /// </summary>
    internal class HtmlNotificationFormatter : INotificationFormatter
    {
        private readonly IUrlGenerator urlGenerator;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Tauchbolde.Common.DomainServices.Notifications.HtmlNotificationFormatter"/> class.
        /// </summary>
        /// <param name="urlGenerator">URL generator.</param>
        public HtmlNotificationFormatter(IUrlGenerator urlGenerator)
        {
            this.urlGenerator = urlGenerator ?? throw new ArgumentNullException(nameof(urlGenerator));
        }

        /// <inheritdoc />
        public string Format(Diver recipient, IEnumerable<Notification> notifications)
        {
            if (recipient == null) throw new ArgumentNullException(nameof(recipient));
            if (notifications == null) throw new ArgumentNullException(nameof(notifications));

            var sb = new StringBuilder();

            FormatHeader(sb, recipient, notifications.Count());
            FormatNotification(sb, recipient, notifications);
            FormatFooter(sb);

            return sb.ToString();
        }

        private void FormatHeader(StringBuilder sb, Diver recipient, int notificationCount)
        {
            sb.AppendLine($"<h2>Hallo {recipient.Firstname},</h2>");

            sb.AppendLine("<p>");
            sb.AppendLine($"Auf der Tauchbolde-Webseite gibt es {notificationCount} News.");
            sb.AppendLine("</p>");
        }

        private void FormatNotification(StringBuilder sb, Diver recipient, IEnumerable<Notification> notifications)
        {
            sb.AppendLine("<ul>");

            foreach (var notification in notifications)
            {
                sb.AppendLine("<li>");

                sb.Append("<span style='font-size: small; color: grey;'>");
                sb.Append(notification.OccuredAt.ToString("dd.MM.yyyy HH.mm "));
                sb.Append("</span>");
                sb.Append(NotificationTypeToString(notification.Type));
                sb.Append(": ");
                sb.Append(notification.Message);

                if (notification.EventId != Guid.Empty)
                {
                    var eventUrl = urlGenerator.GenerateEventUrl(notification.EventId);
                    sb.Append($" <a href='{eventUrl}'>Mehr...</a>");
                }
                                
                sb.AppendLine();

                sb.AppendLine("</li>");
            }

            sb.AppendLine("</ul>");
        }

        private static void FormatFooter(StringBuilder sb)
        {
            sb.AppendLine("<p>");
            sb.AppendLine("Guet Gas!");
            sb.AppendLine("</p>");
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