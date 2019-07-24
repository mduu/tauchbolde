using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using Microsoft.Extensions.Logging;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.Application.Services.PhotoStores;
using Tauchbolde.Domain.Entities;
using Tauchbolde.Domain.Types;
using Tauchbolde.Domain.ValueObjects;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Application.UseCases.Logbook.EditUseCase
{
    [UsedImplicitly]
    internal class EditLogbookEntryHandler : IRequestHandler<EditLogbookEntry, UseCaseResult<LogbookEntry>>
    {
        private readonly ILogger<EditLogbookEntryHandler> logger;
        private readonly IPhotoService photoService;
        private readonly ILogbookEntryRepository repository;

        public EditLogbookEntryHandler(
            [NotNull] ILogger<EditLogbookEntryHandler> logger,
            [NotNull] IPhotoService photoService,
            [NotNull] ILogbookEntryRepository repository)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.photoService = photoService ?? throw new ArgumentNullException(nameof(photoService));
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<UseCaseResult<LogbookEntry>> Handle([NotNull] EditLogbookEntry request,
            CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            
            logger.LogInformation("Editing existing LogbookEntry with Id [{id}] and title [{title}]",
                request.LogbookEntryId, request.Title);

            var existingLogbookEntry = await repository.FindByIdAsync(request.LogbookEntryId);
            if (existingLogbookEntry == null)
            {
                logger.LogError("Error editing logbook-entry because the entry with with ID [{}] can not be found!",
                    request.LogbookEntryId);
            }

            PhotoAndThumbnailIdentifiers teaserIdentifiers = null;
            if (request.TeaserImage != null && request.TeaserImageContentType != null)
            {
                logger.LogInformation("Storing teaser image in photo store: [{filename}]", request.TeaserImageFileName);

                teaserIdentifiers = await photoService.AddPhotoAsync(
                    PhotoCategory.LogbookTeaser,
                    request.TeaserImage,
                    request.TeaserImageContentType,
                    request.TeaserImageFileName);

                logger.LogInformation("Teaser image stored in photo store: [{filename}]", request.TeaserImageFileName);
            }

            existingLogbookEntry.Edit(
                request.Title,
                request.Teaser,
                request.Text,
                request.IsFavorite,
                request.EditorDiverId,
                request.ExternalPhotoAlbumUrl,
                request.RelatedEventId,
                teaserIdentifiers);

            await repository.UpdateAsync(existingLogbookEntry);

            logger.LogInformation(
                "LogbookEntry with ID [{id}] and title [{title}] editeted successfully.",
             existingLogbookEntry.Id, existingLogbookEntry.Title);
            return UseCaseResult<LogbookEntry>.Success(existingLogbookEntry);
        }
    }
}