using System.Text;
using System.Threading.Tasks;

namespace Tauchbolde.Common.DomainServices.Notifications.HtmlFormatting
{
    public interface ICssStyleFormatter
    {
        Task FormatCssStylesAsync(StringBuilder htmlBuilder);
    }
}