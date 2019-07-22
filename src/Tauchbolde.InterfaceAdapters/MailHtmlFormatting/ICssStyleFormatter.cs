using System.Text;
using System.Threading.Tasks;

namespace Tauchbolde.InterfaceAdapters.MailHtmlFormatting
{
    public interface ICssStyleFormatter
    {
        Task FormatCssStylesAsync(StringBuilder htmlBuilder);
    }
}