using System;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Html;
using Tauchbolde.Common.Domain.TextFormatting;

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
            return new HtmlString(textFormatter.GetHtmlText(sourceText));
        }

    }
}