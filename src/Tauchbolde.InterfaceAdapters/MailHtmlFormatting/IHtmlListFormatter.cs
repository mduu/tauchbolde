using System.Text;
using Tauchbolde.Domain.Entities;

namespace Tauchbolde.InterfaceAdapters.MailHtmlFormatting
{
    /// <summary>
    /// Formats a series of <see cref="Notification"/>.
    /// </summary>
    public interface IHtmlListFormatter
    {
        /// <summary>
        /// Format the <paramref name="notifications"/> as HTML and write the
        /// HTML into <paramref name="htmlBuilder"/>.
        /// </summary>
        /// <param name="notifications">The list of notifications to format.</param>
        /// <param name="htmlBuilder">The <see cref="StringBuilder"/> to write the HTML into.</param>
        void Format(IEnumerable<Notification> notifications, StringBuilder htmlBuilder);
    }
}