using System.Collections.Generic;
using System.Text;
using Tauchbolde.Common.Model;

namespace Tauchbolde.Common.DomainServices.Notifications.HtmlFormatting
{
    /// <summary>
    /// Formats a series of <see cref="Notification"/>.
    /// </summary>
    public interface IHtmlNotificationListFormatter
    {
        /// <summary>
        /// Format the <paramref name="notifications"/> as HTML and write the
        /// HTML into <paramref name="htmlBuilder"/>.
        /// </summary>
        /// <param name="notifications"></param>
        /// <param name="htmlBuilder"></param>
        void Format(IEnumerable<Notification> notifications, StringBuilder htmlBuilder);
    }
}