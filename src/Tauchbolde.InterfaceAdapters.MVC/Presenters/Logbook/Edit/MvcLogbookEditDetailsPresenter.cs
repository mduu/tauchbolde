using System;
using Tauchbolde.Application.UseCases.Logbook.GetDetailsUseCase;

namespace Tauchbolde.InterfaceAdapters.MVC.Presenters.Logbook.Edit
{
    public class MvcLogbookEditDetailsPresenter : ILogbookDetailOutputPort
    {
        private LogbookEditViewModel viewModel;

        public void Output(GetLogbookEntryDetailOutput interactorOutput)
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