using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using Tauchbolde.Common.Model;

namespace Tauchbolde.Common.DomainServices.Notifications.HtmlFormatting
{
    /// <summary>
    /// Formats notification for a given recipient as HTML.
    /// </summary>
    /// <inheritdoc />
    internal class HtmlNotificationFormatter : INotificationFormatter
    {
        [NotNull] private readonly IHtmlNotificationListFormatter notificationListFormatter;

        public HtmlNotificationFormatter([NotNull] IHtmlNotificationListFormatter notificationListFormatter)
        {
            this.notificationListFormatter = notificationListFormatter ?? throw new ArgumentNullException(nameof(notificationListFormatter));
        }

        /// <inheritdoc />
        public string Format(Diver recipient, IEnumerable<Notification> notifications)
        {
            if (recipient == null) throw new ArgumentNullException(nameof(recipient));
            if (notifications == null) throw new ArgumentNullException(nameof(notifications));

            var sb = new StringBuilder();

            FormatHeader(sb, recipient, notifications.Count());
            notificationListFormatter.Format(notifications, sb);
            FormatFooter(sb);

            return sb.ToString();
        }

        private static void FormatHeader(StringBuilder sb, Diver recipient, int notificationCount)
        {
            sb.AppendLine($"<h2>Hallo {recipient.Firstname},</h2>");

            sb.AppendLine("<p>");
            sb.AppendLine($"Auf der Tauchbolde-Webseite gibt es {notificationCount} News.");
            sb.AppendLine("</p>");
        }

        private static void FormatFooter(StringBuilder sb)
        {
            sb.AppendLine("<p>");
            sb.AppendLine("Guet Gas!");
            sb.AppendLine("</p>");
        }
    }
}