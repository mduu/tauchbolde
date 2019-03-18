namespace Tauchbolde.Common.DomainServices.TextFormatting
{
    /// <summary>
    /// Defines the interface to format texts.
    /// </summary>
    public interface ITextFormatter
    {
        /// <summary>
        /// Formats the <paramref name="sourceText"/> into HTML.
        /// </summary>
        /// <param name="sourceText"></param>
        /// <returns>The HTML version of the text.</returns>
        string GetHtmlText(string sourceText);
    }
}