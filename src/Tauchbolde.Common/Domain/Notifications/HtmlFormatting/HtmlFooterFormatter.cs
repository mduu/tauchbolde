using System.Text;

namespace Tauchbolde.Common.Domain.Notifications.HtmlFormatting
{
    /// <inheritdoc />
    public class HtmlFooterFormatter : IHtmlFooterFormatter
    {
        /// <inheritdoc />
        public void Format(StringBuilder htmlBuilder)
        {
            htmlBuilder.AppendLine("<p>");
            htmlBuilder.AppendLine("Guet Gas!");
            htmlBuilder.AppendLine("</p>");
        }
    }
}