using System;
using JetBrains.Annotations;
using Tauchbolde.Application.UseCases.Logbook.GetDetailsUseCase;
using Tauchbolde.SharedKernel.Extensions;

namespace Tauchbolde.InterfaceAdapters.Logbook.Details
{
    public class MvcLogbookDetailsOutputPort : ILogbookDetailOutputPort
    {
        private readonly IRelativeUrlGenerator relativeUrlGenerator;
        private readonly ILogbookDetailsUrlGenerator detailsUrlGenerator;
        private LogbookDetailViewModel viewModel;

        public MvcLogbookDetailsOutputPort(
            [NotNull] IRelativeUrlGenerator relativeUrlGenerator,
            [NotNull] ILogbookDetailsUrlGenerator detailsUrlGenerator)
        {
            this.relativeUrlGenerator = relativeUrlGenerator ?? throw new ArgumentNullException(nameof(this.relativeUrlGenerator));
            this.detailsUrlGenerator = detailsUrlGenerator ?? throw new ArgumentNullException(nameof(detailsUrlGenerator));
        }
        
        public void Output([NotNull] GetLogbookEntryDetailOutput interactorOutput)
        {
            if (interactorOutput == null) throw new ArgumentNullException(nameof(interactorOutput));

            viewModel = new LogbookDetailViewModel(
                interactorOutput.AllowEdit,
                interactorOutput.LogbookEntryId,
                interactorOutput.Title,
                interactorOutput.IsFavorite,
                interactorOutput.IsPublished,
                interactorOutput.Teaser,
                interactorOutput.Text,
                interactorOutput.ExternalPhotoAlbumUrl,
                interactorOutput.EventName,
                interactorOutput.OriginalAuthorName,
                interactorOutput.OriginalAuthorEmail,
                interactorOutput.OriginalAuthorAvatarId,
                interactorOutput.EditorName,
                interactorOutput.EditorEmail,
                interactorOutput.EditorAvatarId,
                interactorOutput.CreatedAt.ToStringSwissDateTime(),
                interactorOutput.ModifiedAt?.ToStringSwissDateTime(),
                detailsUrlGenerator.GenerateEditUrl(interactorOutput.LogbookEntryId),
                detailsUrlGenerator.GenerateUnPublishUrl(interactorOutput.LogbookEntryId),
                detailsUrlGenerator.GeneratePublishUrl(interactorOutput.LogbookEntryId),
                detailsUrlGenerator.GenerateDeleteUrl(interactorOutput.LogbookEntryId),
                relativeUrlGenerator.GenerateEventUrl(interactorOutput.EventId),
                detailsUrlGenerator.GenerateTeaserImageUrl(interactorOutput.TeaserImageIdentifier)
            );
        }

        public LogbookDetailViewModel GetViewModel() => viewModel;
    }
}