using System;
using System.Threading.Tasks;
using Tauchbolde.Application.UseCases.Logbook.GetDetailsUseCase;

namespace Tauchbolde.InterfaceAdapters.Logbook.Edit
{
    public class MvcLogbookEditDetailsPresenter : ILogbookDetailPresenter
    {
        private LogbookEditViewModel viewModel;

        public async Task PresentAsync(GetLogbookEntryDetailOutput interactorOutput)
        {
            if (interactorOutput == null) throw new ArgumentNullException(nameof(interactorOutput));
            
            viewModel = new LogbookEditViewModel(
                interactorOutput.LogbookEntryId,
                interactorOutput.IsFavorite,
                null,
                interactorOutput.Title,
                interactorOutput.Teaser,
                interactorOutput.Text,
                interactorOutput.ExternalPhotoAlbumUrl,
                interactorOutput.CreatedAt);
        }
        
        public LogbookEditViewModel GetViewModel() => viewModel;
    }
}