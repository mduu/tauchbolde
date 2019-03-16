using System.Text;
using Tauchbolde.Common.Model;

namespace Tauchbolde.Common.DomainServices.Notifications.HtmlFormatting
{
    public class HtmlHeaderFormatter : IHtmlHeaderFormatter
    {
        public void Format(Diver recipient, int notificationCount, StringBuilder htmlString)
        {
            htmlString.AppendLine($"<h2>Hallo {recipient.Firstname},</h2>");

            htmlString.AppendLine("<p>");
            htmlString.AppendLine($"Auf der Tauchbolde-Webseite gibt es {notificationCount} News.");
            htmlString.AppendLine("</p>");
        }

    }
}