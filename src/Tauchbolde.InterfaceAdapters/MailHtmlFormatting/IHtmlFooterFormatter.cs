using System.Text;

namespace Tauchbolde.InterfaceAdapters.MailHtmlFormatting
{
    /// <summary>
    /// Format the HTML footer.
    /// </summary>
    public interface IHtmlFooterFormatter
    {
        /// <summary>
        /// Format the footer as HTML and writes it into <paramref name="htmlBuilder"/>.
        /// </summary>
        /// <param name="htmlBuilder">The <see cref="StringBuilder"/> to write the HTML into.</param>
        void Format(StringBuilder htmlBuilder);
    }
}