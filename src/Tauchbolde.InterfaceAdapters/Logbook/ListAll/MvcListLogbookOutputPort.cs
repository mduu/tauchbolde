using System;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Tauchbolde.Application.UseCases.Logbook.ListAllUseCase;
using Tauchbolde.InterfaceAdapters.TextFormatting;

namespace Tauchbolde.InterfaceAdapters.Logbook.ListAll
{
    public class MvcListLogbookOutputPort : IListLogbookEntriesOutputPort
    {
        private const int TeaserLength = 250;
        private readonly bool allowEdit;
        private readonly ITextFormatter textFormatter;
        private LogbookListViewModel viewModel;

        public MvcListLogbookOutputPort(bool allowEdit, [NotNull] ITextFormatter textFormatter)
        {
            this.allowEdit = allowEdit;
            this.textFormatter = textFormatter ?? throw new ArgumentNullException(nameof(textFormatter));
        }
        
        public void Output([NotNull] ListAllLogbookEntriesOutputPort output)
        {
            if (output == null) throw new ArgumentNullException(nameof(output));
            
            viewModel = new LogbookListViewModel(
                allowEdit,
                output.LogbookItems.Select(l => new LogbookListViewModel.LogbookItemViewModel(
                    l.LogbookEntryId,
                    l.Title,
                    GetFormattedTeaserText(l),
                    l.TeaserImageUrl,
                    l.IsPublished,
                    l.Text))
                );
        }

        public LogbookListViewModel GetViewModel() => viewModel;
        
        private string GetFormattedTeaserText(ListAllLogbookEntriesOutputPort.LogbookItem logbookItem)
        {
            var teaserText = GetTeaserText(logbookItem);
            
            return string.IsNullOrWhiteSpace(teaserText)
                ? teaserText
                : textFormatter.GetHtmlText(teaserText);
        }

        private string GetTeaserText(ListAllLogbookEntriesOutputPort.LogbookItem logbookItem)
        {
            if (!string.IsNullOrWhiteSpace(logbookItem.TeaserText))
            {
                return logbookItem.TeaserText;
            }

            return !string.IsNullOrWhiteSpace(logbookItem.Text) && logbookItem.Text.Length > TeaserLength
                ? $"{logbookItem.Text.Substring(0, TeaserLength)}..."
                : logbookItem.Text;
        }
    }
}