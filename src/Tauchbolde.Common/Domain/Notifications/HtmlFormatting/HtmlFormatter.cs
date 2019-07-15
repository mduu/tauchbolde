using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Tauchbolde.Entities;

namespace Tauchbolde.Common.Domain.Notifications.HtmlFormatting
{
    /// <summary>
    /// Formats notification for a given recipient as HTML.
    /// </summary>
    /// <inheritdoc />
    internal class HtmlFormatter : INotificationFormatter
    {
        [NotNull] private readonly ICssStyleFormatter styleFormatter;
        [NotNull] private readonly IHtmlHeaderFormatter headerFormatter;
        [NotNull] private readonly IHtmlListFormatter listFormatter;
        [NotNull] private readonly IHtmlFooterFormatter footerFormatter;

        public HtmlFormatter(
            [NotNull] ICssStyleFormatter styleFormatter,
            [NotNull] IHtmlHeaderFormatter headerFormatter,
            [NotNull] IHtmlListFormatter listFormatter,
            [NotNull] IHtmlFooterFormatter footerFormatter)
        {
            this.styleFormatter = styleFormatter ?? throw new ArgumentNullException(nameof(styleFormatter));
            this.headerFormatter = headerFormatter ?? throw new ArgumentNullException(nameof(headerFormatter));
            this.listFormatter = listFormatter ?? throw new ArgumentNullException(nameof(listFormatter));
            this.footerFormatter = footerFormatter ?? throw new ArgumentNullException(nameof(footerFormatter));
        }

        /// <inheritdoc />
        public async Task<string> FormatAsync(Diver recipient, IEnumerable<Notification> notifications)
        {
            if (recipient == null) throw new ArgumentNullException(nameof(recipient));
            if (notifications == null) throw new ArgumentNullException(nameof(notifications));

            var htmlBuilder = new StringBuilder();
            await FormatHead(htmlBuilder);
            FormatBody(recipient, notifications, htmlBuilder);

            return htmlBuilder.ToString();
        }
        
        private async Task FormatHead(StringBuilder htmlBuilder)
        {
            htmlBuilder.AppendLine("<html>");
            htmlBuilder.AppendLine("<head>");
            await styleFormatter.FormatCssStylesAsync(htmlBuilder);
            htmlBuilder.AppendLine("</head>");
        }

        private void FormatBody(Diver recipient, IEnumerable<Notification> notifications, StringBuilder htmlBuilder)
        {
            htmlBuilder.AppendLine("<body>");
            headerFormatter.Format(recipient, notifications.Count(), htmlBuilder);
            listFormatter.Format(notifications, htmlBuilder);
            footerFormatter.Format(htmlBuilder);
            htmlBuilder.AppendLine("</html>");
            htmlBuilder.AppendLine("</body>");
        }
    }
}