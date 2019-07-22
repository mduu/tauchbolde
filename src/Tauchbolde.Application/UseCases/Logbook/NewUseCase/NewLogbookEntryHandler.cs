using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using Microsoft.Extensions.Logging;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.Application.OldDomainServices.PhotoStorage;
using Tauchbolde.Domain.Entities;
using Tauchbolde.Domain.Types;
using Tauchbolde.Domain.ValueObjects;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Application.UseCases.Logbook.NewUseCase
{
    [UsedImplicitly]
    public class NewLogbookEntryHandler : IRequestHandler<NewLogbookEntry, UseCaseResult>
    {
        private readonly ILogger<NewLogbookEntryHandler> logger;
        private readonly ILogbookEntryRepository repository;
        private readonly IPhotoService photoService;

        public NewLogbookEntryHandler(
            [NotNull] ILogger<NewLogbookEntryHandler> logger,
            [NotNull] ILogbookEntryRepository repository,
            [NotNull] IPhotoService photoService)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.photoService = photoService ?? throw new ArgumentNullException(nameof(photoService));
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

            var logbookEntry = LogbookEntry.CreateNew(
                request.Title,
                request.Teaser,
                request.Text,
                request.IsFavorite,
                request.AuthorDiverId,
                request.ExternalPhotoAlbumUrl,
                request.RelatedEventId,
                teaserIdentifiers);

            var newLogbookEntry = await repository.InsertAsync(logbookEntry);
            
            logger.LogInformation("New LogbookEntry for [{title}] created with Id [{}]", request.Title, newLogbookEntry.Id);
            throw new System.NotImplementedException();
        }
    }
}