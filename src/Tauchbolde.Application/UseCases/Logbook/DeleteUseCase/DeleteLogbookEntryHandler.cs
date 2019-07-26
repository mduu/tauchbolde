using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation.Results;
using JetBrains.Annotations;
using MediatR;
using Microsoft.Extensions.Logging;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.Application.Services.PhotoStores;
using Tauchbolde.Domain.Entities;
using Tauchbolde.Domain.ValueObjects;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Application.UseCases.Logbook.DeleteUseCase
{
    [UsedImplicitly]
    public class DeleteLogbookEntryHandler : IRequestHandler<DeleteLogbookEntry, UseCaseResult>
    {
        private readonly ILogger<DeleteLogbookEntryHandler> logger;
        private readonly ILogbookEntryRepository repository;
        private readonly IPhotoService photoService;

        public DeleteLogbookEntryHandler(
            [NotNull] ILogger<DeleteLogbookEntryHandler> logger,
            [NotNull] ILogbookEntryRepository repository,
            [NotNull] IPhotoService photoService)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.photoService = photoService ?? throw new ArgumentNullException(nameof(photoService));
        }
        
        public async Task<UseCaseResult> Handle([NotNull] DeleteLogbookEntry request, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            logger.LogInformation("Deleting LogbookEntry with Id [{id}]", request.LogbookEntryId);

            var existingLogbookEntry = await repository.FindByIdAsync(request.LogbookEntryId);
            if (existingLogbookEntry == null)
            {
                return UseCaseResult.Fail(new List<ValidationFailure>
                {
                    new ValidationFailure(nameof(request.LogbookEntryId), $"Logbookeintrag [{request.LogbookEntryId}] nicht gefunden!")
                });
            }

            existingLogbookEntry.Delete();

            await repository.DeleteAsync(existingLogbookEntry);
            await RemoveTeaserPhotosAsync(existingLogbookEntry);

            logger.LogInformation("Deleted LogbookEntry with Id [{id}] successfully", request.LogbookEntryId);
            return UseCaseResult.Success();
        }

        private async Task RemoveTeaserPhotosAsync(LogbookEntry existingLogbookEntry)
        {
            var identifiersToDelete = new List<PhotoIdentifier>();

            if (!string.IsNullOrWhiteSpace(existingLogbookEntry.TeaserImage))
            {
                identifiersToDelete.Add(new PhotoIdentifier(existingLogbookEntry.TeaserImage));
            }

            if (!string.IsNullOrWhiteSpace(existingLogbookEntry.TeaserImageThumb))
            {
                identifiersToDelete.Add(new PhotoIdentifier(existingLogbookEntry.TeaserImageThumb));
            }

            await photoService.RemovePhotosAsync(identifiersToDelete.ToArray());
        }
    }
}