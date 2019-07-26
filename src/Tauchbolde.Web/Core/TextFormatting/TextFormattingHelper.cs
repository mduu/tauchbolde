using System;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Html;
using Tauchbolde.InterfaceAdapters.TextFormatting;

namespace Tauchbolde.Web.Core.TextFormatting
{
    internal class TextFormattingHelper : ITextFormattingHelper
    {
        private readonly ITextFormatter textFormatter;

        public TextFormattingHelper([NotNull] ITextFormatter textFormatter)
        {
            this.textFormatter = textFormatter ?? throw new ArgumentNullException(nameof(textFormatter));
        }
        
        public HtmlString FormatText(string sourceText)
        {
            return string.IsNullOrWhiteSpace(sourceText) 
                ? new HtmlString("")
                : new HtmlString(textFormatter.GetHtmlText(sourceText));
        }

    }
}