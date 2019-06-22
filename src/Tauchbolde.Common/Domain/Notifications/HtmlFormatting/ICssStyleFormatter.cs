using System.Text;
using System.Threading.Tasks;

namespace Tauchbolde.Common.Domain.Notifications.HtmlFormatting
{
    public interface ICssStyleFormatter
    {
        Task FormatCssStylesAsync(StringBuilder htmlBuilder);
    }
}