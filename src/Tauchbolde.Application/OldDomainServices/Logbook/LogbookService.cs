using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.Application.OldDomainServices.Notifications;
using Tauchbolde.Application.OldDomainServices.Users;
using Tauchbolde.Application.Services;
using Tauchbolde.Application.Services.PhotoStores;
using Tauchbolde.Domain.Entities;
using Tauchbolde.Domain.ValueObjects;

namespace Tauchbolde.Application.OldDomainServices.Logbook
{
    /// <summary>
    /// Standard implementation of <see cref="ILogbookService"/>.
    /// </summary>
    internal class LogbookService : ILogbookService
    {
        [NotNull] private readonly ILogbookEntryRepository logbookEntryRepository;
        [NotNull] private readonly ITelemetryService telemetryService;
        [NotNull] private readonly IPhotoService photoService;

        public LogbookService(
            [NotNull] ILogbookEntryRepository logbookEntryRepository,
            [NotNull] IDiverService diverService,
            [NotNull] ITelemetryService telemetryService,
            [NotNull] IPhotoService photoService,
            [NotNull] ILogger<LogbookService> logger,
            [NotNull] INotificationService notificationService)
        {
            this.logbookEntryRepository = logbookEntryRepository ?? throw new ArgumentNullException(nameof(logbookEntryRepository));
            this.telemetryService = telemetryService ?? throw new ArgumentNullException(nameof(telemetryService));
            this.photoService = photoService ?? throw new ArgumentNullException(nameof(photoService));
        }

        /// <param name="includeUnpublished"></param>
        /// <inheritdoc />
        public async Task<ICollection<LogbookEntry>> GetAllEntriesAsync(bool includeUnpublished = false)
            => await logbookEntryRepository.GetAllEntriesAsync(includeUnpublished);

        /// <inheritdoc />
        public async Task<LogbookEntry> FindByIdAsync(Guid logbookEntryId)
            => await logbookEntryRepository.FindByIdAsync(logbookEntryId);
        
        /// <inheritdoc />
        public async Task DeleteAsync(Guid logbookEntryId)
        {
            if (logbookEntryId == Guid.Empty) { throw new ArgumentException("Must be not Guid.Empty!", nameof(logbookEntryId)); }

            var logbookEntry = await logbookEntryRepository.FindByIdAsync(logbookEntryId);
            if (logbookEntry == null)
            {
                throw new InvalidOperationException($"No {nameof(LogbookEntry)} with Id [{logbookEntryId}] found!");
            }
                        
            await logbookEntryRepository.DeleteAsync(logbookEntry);
            await RemoveTeaserImagesAsync(logbookEntry);
            
            TrackLogbookEntry("LOGBOOK-DELETE", logbookEntry);
        }

        /// <inheritdoc />
        public async Task<Photo> GetPhotoDataAsync(PhotoIdentifier photoIdentifier)
        {
            if (photoIdentifier == null) throw new ArgumentNullException(nameof(photoIdentifier));

            return await photoService.GetPhotoDataAsync(photoIdentifier);
        }

        private async Task RemoveTeaserImagesAsync([NotNull] LogbookEntry logbookEntry)
        {
            if (logbookEntry == null) throw new ArgumentNullException(nameof(logbookEntry));

            var identifiersToDelete = new List<PhotoIdentifier>();
            
            if (!string.IsNullOrWhiteSpace(logbookEntry.TeaserImage))
            {
                identifiersToDelete.Add(new PhotoIdentifier(logbookEntry.TeaserImage));
            }

            if (!string.IsNullOrWhiteSpace(logbookEntry.TeaserImageThumb))
            {
                identifiersToDelete.Add(new PhotoIdentifier(logbookEntry.TeaserImageThumb));
            }
            
            await photoService.RemovePhotosAsync(identifiersToDelete.ToArray());

        }

        private void TrackLogbookEntry(string name, LogbookEntry logbookEntryToTrack)
        {
            if (logbookEntryToTrack == null) { throw new ArgumentNullException(nameof(logbookEntryToTrack)); }

            telemetryService.TrackEvent(
                name,
                new Dictionary<string, string>
                {
                    {"EventId", logbookEntryToTrack.EventId?.ToString("B") ?? ""},
                    {"CommentId", logbookEntryToTrack.Id.ToString("B")},
                    {"AuthorId", logbookEntryToTrack.OriginalAuthorId.ToString("B")},
                    {"Title", logbookEntryToTrack.Title},
                    {"CreateDate", logbookEntryToTrack.CreatedAt.ToString("O")},
                    {"ModifiedAt", logbookEntryToTrack.ModifiedAt?.ToString("O") ?? ""}
                });
        }

    }
}