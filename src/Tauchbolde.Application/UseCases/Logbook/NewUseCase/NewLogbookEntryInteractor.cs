using JetBrains.Annotations;
using MediatR;
using Microsoft.Extensions.Logging;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.Application.Services.Core;
using Tauchbolde.Application.Services.PhotoStores;
using Tauchbolde.Domain.Entities;
using Tauchbolde.Domain.Types;
using Tauchbolde.Domain.ValueObjects;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Application.UseCases.Logbook.NewUseCase
{
    [UsedImplicitly]
    internal class NewLogbookEntryInteractor : IRequestHandler<NewLogbookEntry, UseCaseResult>
    {
        [NotNull] private readonly ILogger<NewLogbookEntryInteractor> logger;
        [NotNull] private readonly ILogbookEntryRepository repository;
        [NotNull] private readonly IPhotoService photoService;
        [NotNull] private readonly ICurrentUser currentUser;

        public NewLogbookEntryInteractor(
            [NotNull] ILogger<NewLogbookEntryInteractor> logger,
            [NotNull] ILogbookEntryRepository repository,
            [NotNull] IPhotoService photoService,
            [NotNull] ICurrentUser currentUser)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.photoService = photoService ?? throw new ArgumentNullException(nameof(photoService));
            this.currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        }
        
        public async Task<UseCaseResult> Handle([NotNull] NewLogbookEntry request, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            
            logger.LogInformation("Creating new LogbookEntry for [{title}]", request.Title);

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

            var currentDiver = await currentUser.GetCurrentDiverAsync();
            if (currentDiver == null)
            {
                logger.LogError("Could not get diver for current user!");
                return UseCaseResult.Fail();
            }

            var logbookEntry = new LogbookEntry(
                request.Title,
                request.Teaser,
                request.Text,
                request.IsFavorite,
                currentDiver.Id,
                request.ExternalPhotoAlbumUrl,
                request.RelatedEventId,
                teaserIdentifiers);

            var newLogbookEntry = await repository.InsertAsync(logbookEntry);
            
            logger.LogInformation("New LogbookEntry for [{title}] created with Id [{id}]", request.Title, newLogbookEntry.Id);
            return UseCaseResult.Success();
        }
    }
}