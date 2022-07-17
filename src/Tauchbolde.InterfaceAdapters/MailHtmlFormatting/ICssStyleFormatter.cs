using System.Text;

namespace Tauchbolde.InterfaceAdapters.MailHtmlFormatting
{
    public interface ICssStyleFormatter
    {
        Task FormatCssStylesAsync(StringBuilder htmlBuilder);
    }
}