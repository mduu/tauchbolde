using System;
using Markdig;

namespace Tauchbolde.Common.Domain.TextFormatting
{
    /// <summary>
    /// Implementation of <see cref="T:Tauchbolde.Common.Domain.TextFormatting.ITextFormatter" />
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