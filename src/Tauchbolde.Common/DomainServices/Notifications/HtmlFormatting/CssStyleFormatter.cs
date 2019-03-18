using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Tauchbolde.Common.DomainServices.Notifications.HtmlFormatting
{
    public class CssStyleFormatter : ICssStyleFormatter
    {
        public async Task FormatCssStylesAsync(StringBuilder htmlBuilder)
        {
            var cssSource = "";
            var currentAssembly = typeof(CssStyleFormatter).Assembly;
            using (var stream = currentAssembly.GetManifestResourceStream(
                "Tauchbolde.Common.DomainServices.Notifications.HtmlFormatting.EmbeddedStyles.css"))
            {
                using (var reader = new StreamReader(stream, Encoding.UTF8))
                {
                    cssSource = await reader.ReadToEndAsync();
                }
            }
            
            htmlBuilder.Append(cssSource);
        }
    }
}