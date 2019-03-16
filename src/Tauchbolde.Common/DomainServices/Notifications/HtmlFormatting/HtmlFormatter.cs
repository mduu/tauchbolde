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
    internal class HtmlFormatter : INotificationFormatter
    {
        [NotNull] private readonly IHtmlHeaderFormatter headerFormatter;
        [NotNull] private readonly IHtmlListFormatter listFormatter;
        [NotNull] private readonly IHtmlFooterFormatter footerFormatter;

        public HtmlFormatter(
            [NotNull] IHtmlHeaderFormatter headerFormatter,
            [NotNull] IHtmlListFormatter listFormatter,
            [NotNull] IHtmlFooterFormatter footerFormatter)
        {
            this.headerFormatter = headerFormatter ?? throw new ArgumentNullException(nameof(headerFormatter));
            this.listFormatter = listFormatter ?? throw new ArgumentNullException(nameof(listFormatter));
            this.footerFormatter = footerFormatter ?? throw new ArgumentNullException(nameof(footerFormatter));
        }

        /// <inheritdoc />
        public string Format(Diver recipient, IEnumerable<Notification> notifications)
        {
            if (recipient == null) throw new ArgumentNullException(nameof(recipient));
            if (notifications == null) throw new ArgumentNullException(nameof(notifications));

            var htmlBuilder = new StringBuilder();

            headerFormatter.Format(recipient, notifications.Count(), htmlBuilder);
            listFormatter.Format(notifications, htmlBuilder);
            footerFormatter.Format(htmlBuilder);

            return htmlBuilder.ToString();
        }
    }
}