using System.Text;

namespace Tauchbolde.InterfaceAdapters.MailHtmlFormatting
{
    public class CssStyleFormatter : ICssStyleFormatter
    {
        public async Task FormatCssStylesAsync(StringBuilder htmlBuilder)
        {
            var cssSource = "";
            var currentAssembly = typeof(CssStyleFormatter).Assembly;
            using (var stream = currentAssembly.GetManifestResourceStream(
                "Tauchbolde.InterfaceAdapters.MailHtmlFormatting.EmbeddedStyles.css"))
            {
                using (var reader = new StreamReader(stream, Encoding.UTF8))
                {
                    cssSource = await reader.ReadToEndAsync();
                }
            }

            htmlBuilder.AppendLine("<style>");
            htmlBuilder.Append(cssSource);
            htmlBuilder.AppendLine("</style>");
        }
    }
}