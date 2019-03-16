using System.Text;
using Tauchbolde.Common.Model;

namespace Tauchbolde.Common.DomainServices.Notifications.HtmlFormatting
{
    public interface IHtmlHeaderFormatter
    {
        void Format(Diver recipient, int notificationCount, StringBuilder htmlString);
    }
}