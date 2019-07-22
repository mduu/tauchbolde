using System.Text;
using Tauchbolde.Domain.Entities;

namespace Tauchbolde.InterfaceAdapters.MailHtmlFormatting
{
    public class HtmlHeaderFormatter : IHtmlHeaderFormatter
    {
        public void Format(Diver recipient, int notificationCount, StringBuilder htmlBuilder)
        {
            htmlBuilder.AppendLine($"<h2>Hallo {recipient.Firstname},</h2>");

            htmlBuilder.AppendLine("<p>");
            htmlBuilder.AppendLine($"Auf der Tauchbolde-Webseite gibt es {notificationCount} News.");
            htmlBuilder.AppendLine("</p>");
        }

    }
}