using System;
using Markdig;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Tauchbolde.Web.Core.Markdown
{
    public static class MarkdownFormatter
    {
        static Lazy<MarkdownPipeline> markDigPipeline = 
            new Lazy<MarkdownPipeline>(
                () => new MarkdownPipelineBuilder().UseAdvancedExtensions().Build()
            );
    
        public static HtmlString Markdown(this IHtmlHelper htmlHelper, string markdownText)
        {
            var result = Markdig.Markdown.ToHtml(markdownText, markDigPipeline.Value);

            return new HtmlString(result);
        }
    }
}
