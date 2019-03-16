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
        [NotNull] private readonly IHtmlHeaderFormatter headerFormatter;
        [NotNull] private readonly IHtmlNotificationListFormatter notificationListFormatter;

        public HtmlNotificationFormatter(
            [NotNull] IHtmlHeaderFormatter headerFormatter,
            [NotNull] IHtmlNotificationListFormatter notificationListFormatter)
        {
            this.headerFormatter = headerFormatter ?? throw new ArgumentNullException(nameof(headerFormatter));
            this.notificationListFormatter = notificationListFormatter ?? throw new ArgumentNullException(nameof(notificationListFormatter));
        }

        /// <inheritdoc />
        public string Format(Diver recipient, IEnumerable<Notification> notifications)
        {
            if (recipient == null) throw new ArgumentNullException(nameof(recipient));
            if (notifications == null) throw new ArgumentNullException(nameof(notifications));

            var sb = new StringBuilder();

            headerFormatter.Format(recipient, notifications.Count(), sb);
            notificationListFormatter.Format(notifications, sb);
            FormatFooter(sb);

            return sb.ToString();
        }

        private static void FormatFooter(StringBuilder sb)
        {
            sb.AppendLine("<p>");
            sb.AppendLine("Guet Gas!");
            sb.AppendLine("</p>");
        }
    }
}