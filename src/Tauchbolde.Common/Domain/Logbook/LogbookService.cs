using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Tauchbolde.Common.Domain.Notifications;
using Tauchbolde.Common.Domain.PhotoStorage;
using Tauchbolde.Common.Domain.Repositories;
using Tauchbolde.Common.Domain.Users;
using Tauchbolde.Common.Infrastructure.Telemetry;
using Tauchbolde.Common.Model;

namespace Tauchbolde.Common.Domain.Logbook
{
    /// <summary>
    /// Standard implementation of <see cref="ILogbookService"/>.
    /// </summary>
    internal class LogbookService : ILogbookService
    {
        [NotNull] private readonly ILogbookEntryRepository logbookEntryRepository;
        [NotNull] private readonly IDiverService diverService;
        [NotNull] private readonly ITelemetryService telemetryService;
        [NotNull] private readonly IPhotoService photoService;
        [NotNull] private readonly ILogger<LogbookService> logger;
        [NotNull] private readonly INotificationService notificationService;

        public LogbookService(
            [NotNull] ILogbookEntryRepository logbookEntryRepository,
            [NotNull] IDiverService diverService,
            [NotNull] ITelemetryService telemetryService,
            [NotNull] IPhotoService photoService,
            [NotNull] ILogger<LogbookService> logger,
            [NotNull] INotificationService notificationService)
        {
            this.logbookEntryRepository = logbookEntryRepository ?? throw new ArgumentNullException(nameof(logbookEntryRepository));
            this.diverService = diverService ?? throw new ArgumentNullException(nameof(diverService));
            this.telemetryService = telemetryService ?? throw new ArgumentNullException(nameof(telemetryService));
            this.photoService = photoService ?? throw new ArgumentNullException(nameof(photoService));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
        }

        /// <param name="includeUnpublished"></param>
        /// <inheritdoc />
        public async Task<ICollection<LogbookEntry>> GetAllEntriesAsync(bool includeUnpublished = false)
            => await logbookEntryRepository.GetAllEntriesAsync(includeUnpublished);

        /// <inheritdoc />
        public async Task<LogbookEntry> FindByIdAsync(Guid logbookEntryId)
            => await logbookEntryRepository.FindByIdAsync(logbookEntryId);

        /// <inheritdoc />
        public async Task<Guid> UpsertAsync([NotNull] LogbookUpsertModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            ValidateUpsertModel(model);

            return model.Id.HasValue
                ? await UpdateExistingLogbookEntryAsync(model)
                : await InsertNewLogbookEntryAsync(model);
        }

        /// <inheritdoc />
        public async Task DeleteAsync(Guid logbookEntryId)
        {
            if (logbookEntryId == Guid.Empty) { throw new ArgumentException("Must be not Guid.Empty!", nameof(logbookEntryId)); }

            var logbookEntry = await logbookEntryRepository.FindByIdAsync(logbookEntryId);
            if (logbookEntry == null)
            {
                throw new InvalidOperationException($"No {nameof(LogbookEntry)} with Id [{logbookEntryId}] found!");
            }
                        
            logbookEntryRepository.Delete(logbookEntry);
            await RemoveTeaserImagesAsync(logbookEntry);
            
            TrackLogbookEntry("LOGBOOK-DELETE", logbookEntry);
        }

        /// <inheritdoc />
        public async Task<Photo> GetPhotoDataAsync(PhotoIdentifier photoIdentifier)
        {
            if (photoIdentifier == null) throw new ArgumentNullException(nameof(photoIdentifier));

            return await photoService.GetPhotoDataAsync(photoIdentifier);
        }

        public async Task PublishAsync(LogbookEntry logbookEntry)
        {
            if (logbookEntry == null) throw new ArgumentNullException(nameof(logbookEntry));
            
            var existingLogbookEntry = await logbookEntryRepository.FindByIdAsync(logbookEntry.Id);
            if (existingLogbookEntry == null)
            {
                throw new InvalidOperationException($"No existing LogbookEntry found with Id [{logbookEntry.Id}]!");
            }

            existingLogbookEntry.IsPublished = true;
            logbookEntryRepository.Update(existingLogbookEntry);
            await notificationService.NotifyForNewLogbookEntry(existingLogbookEntry, existingLogbookEntry.OriginalAuthor);
            
            logger.LogInformation($"Logbook entry [{existingLogbookEntry.Id}] published.");
        }

        public async Task UnPublishAsync(LogbookEntry logbookEntry)
        {
            if (logbookEntry == null) throw new ArgumentNullException(nameof(logbookEntry));
            
            var existingLogbookEntry = await logbookEntryRepository.FindByIdAsync(logbookEntry.Id);
            if (existingLogbookEntry == null)
            {
                throw new InvalidOperationException($"No existing LogbookEntry found with Id [{logbookEntry.Id}]!");
            }

            existingLogbookEntry.IsPublished = false;
            logbookEntryRepository.Update(existingLogbookEntry);
            
            logger.LogInformation($"Logbook entry [{existingLogbookEntry.Id}] un-published.");
        }

        private async Task<Guid> UpdateExistingLogbookEntryAsync([NotNull] LogbookUpsertModel upsertModel)
        {
            if (upsertModel == null) throw new ArgumentNullException(nameof(upsertModel));

            if (!upsertModel.Id.HasValue)
            {
                throw new InvalidOperationException("No existing LogEntry.Id specified.");
            }

            var currentUser = await GetCurrentUserAsync(upsertModel);
            var existingLogbookEntry = await logbookEntryRepository.FindByIdAsync(upsertModel.Id.Value);
            if (existingLogbookEntry == null)
            {
                throw new InvalidOperationException($"No existing LogbookEntry found with Id [{upsertModel.Id}]!");
            }

            PhotoAndThumbnailIdentifiers photoIdentifier = null;
            if (upsertModel.TeaserImage != null)
            {
                photoIdentifier = await photoService.AddPhotoAsync(
                    PhotoCategory.LogbookTeaser,
                    upsertModel.TeaserImage,
                    upsertModel.TeaserImageContentType ?? throw new InvalidOperationException(),
                    upsertModel.TeaserImageFileName);
            }
            
            MapUpsertModelToLogbookEntry(upsertModel, existingLogbookEntry, photoIdentifier);
            existingLogbookEntry.ModifiedAt = DateTime.Now;
            existingLogbookEntry.EditorAuthorId = currentUser.Id;
            
            logbookEntryRepository.Update(existingLogbookEntry);
            TrackLogbookEntry("LOGBOOK-UPDATE", existingLogbookEntry);

            return existingLogbookEntry.Id;
        }

        private async Task<Guid> InsertNewLogbookEntryAsync([NotNull] LogbookUpsertModel upsertModel)
        {
            if (upsertModel == null) throw new ArgumentNullException(nameof(upsertModel));
            
            var currentUser = await GetCurrentUserAsync(upsertModel);
            var newLogbookEntry = new LogbookEntry
            {
                Id = Guid.NewGuid(),
                OriginalAuthorId = currentUser.Id
            };

            
            PhotoAndThumbnailIdentifiers teaserIdentifiers = null;
            if (upsertModel.TeaserImage != null && upsertModel.TeaserImageContentType != null)
            {
                teaserIdentifiers = await photoService.AddPhotoAsync(
                    PhotoCategory.LogbookTeaser,
                    upsertModel.TeaserImage,
                    upsertModel.TeaserImageContentType,
                    upsertModel.TeaserImageFileName);
            }

            MapUpsertModelToLogbookEntry(upsertModel, newLogbookEntry, teaserIdentifiers);
            await logbookEntryRepository.InsertAsync(newLogbookEntry);
            
            TrackLogbookEntry("LOGBOOK-INSERT", newLogbookEntry);

            return newLogbookEntry.Id;
        }

        private static void ValidateUpsertModel(LogbookUpsertModel model)
        {
            if (model.CurrentDiverId == Guid.Empty)
            {
                throw new InvalidOperationException($"{nameof(model.CurrentDiverId)} must not be Guid.Empty!");
            }

            if (string.IsNullOrWhiteSpace(model.Title))
            {
                throw new InvalidOperationException($"{nameof(model.Title)} must not be null or empty!");
            }

            if (string.IsNullOrWhiteSpace(model.Text))
            {
                throw new InvalidOperationException($"{nameof(model.Text)} must not be null or empty!");
            }
        }
        
        private static void MapUpsertModelToLogbookEntry(
            [NotNull] LogbookUpsertModel upsertModel,
            [NotNull] LogbookEntry logbookEntry,
            [CanBeNull] PhotoAndThumbnailIdentifiers teaserIdentifiers)
        {
            if (upsertModel == null) throw new ArgumentNullException(nameof(upsertModel));
            if (logbookEntry == null) throw new ArgumentNullException(nameof(logbookEntry));
            
            logbookEntry.Title = upsertModel.Title;
            logbookEntry.TeaserText = upsertModel.Teaser ?? "";
            logbookEntry.Text = upsertModel.Text;
            logbookEntry.CreatedAt = upsertModel.CreatedAt;
            logbookEntry.ExternalPhotoAlbumUrl = upsertModel.ExternalPhotoAlbumUrl;
            logbookEntry.TeaserImage = teaserIdentifiers?.OriginalPhotoIdentifier?.Serialze() ?? logbookEntry.TeaserImage;
            logbookEntry.TeaserImageThumb = teaserIdentifiers?.ThumbnailPhotoIdentifier?.Serialze() ?? logbookEntry.TeaserImageThumb;
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

        private async Task<Diver> GetCurrentUserAsync(LogbookUpsertModel model)
        {
            var currentUser = await diverService.GetMemberAsync(model.CurrentDiverId);
            if (currentUser == null)
            {
                throw new InvalidOperationException($"No member with Diver-ID [{model.CurrentDiverId}] found!");
            }

            return currentUser;
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