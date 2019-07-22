using System;
using Markdig;

namespace Tauchbolde.InterfaceAdapters.TextFormatting
{
    /// <summary>
    /// Implementation of <see cref="T:Tauchbolde.InterfaceAdapters.TextFormatting.ITextFormatter" />
    /// using MarkdownDig / Markdown.
    /// </summary>
    /// <inheritdoc />
    internal class MarkdownDigFormatter : ITextFormatter
    {
        private static readonly Lazy<MarkdownPipeline> MarkDigPipeline = 
            new Lazy<MarkdownPipeline>(
                () => new MarkdownPipelineBuilder().UseAdvancedExtensions().Build()
            );

        /// <inheritdoc />
        public string GetHtmlText(string sourceText) =>
            Markdown.ToHtml(sourceText, MarkDigPipeline.Value);
    }
}