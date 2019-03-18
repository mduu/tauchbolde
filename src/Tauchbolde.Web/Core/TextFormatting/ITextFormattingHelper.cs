using Microsoft.AspNetCore.Html;

namespace Tauchbolde.Web.Core.TextFormatting
{
    /// <summary>
    /// Provide access to text formatting services.
    /// </summary>
    public interface ITextFormattingHelper
    {
        HtmlString FormatText(string sourceText);
    }
}