using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Tauchbolde.Common.DomainServices.PhotoStorage;
using Tauchbolde.Common.DomainServices.Repositories;
using Tauchbolde.Common.DomainServices.Users;
using Tauchbolde.Common.Infrastructure.Telemetry;
using Tauchbolde.Common.Model;

namespace Tauchbolde.Common.DomainServices.Logbook
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

        public LogbookService(
            [NotNull] ILogbookEntryRepository logbookEntryRepository,
            [NotNull] IDiverService diverService,
            [NotNull] ITelemetryService telemetryService,
            [NotNull] IPhotoService photoService)
        {
            this.logbookEntryRepository = logbookEntryRepository ?? throw new ArgumentNullException(nameof(logbookEntryRepository));
            this.diverService = diverService ?? throw new ArgumentNullException(nameof(diverService));
            this.telemetryService = telemetryService ?? throw new ArgumentNullException(nameof(telemetryService));
            this.photoService = photoService ?? throw new ArgumentNullException(nameof(photoService));
        }

        /// <inheritdoc />
        public async Task<ICollection<LogbookEntry>> GetAllEntriesAsync()
            => await logbookEntryRepository.GetAllEntriesAsync();

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

            PhotoAndThumbnailIdentification photoIdentifier = null;
            if (upsertModel.TeaserImage != null)
            {
                photoIdentifier = await photoService.AddPhotoAsync(
                    PhotoCategory.EventTeaser,
                    upsertModel.TeaserImage,
                    upsertModel.TeaserImageFileName,
                    upsertModel.TeaserImageContentType ?? throw new InvalidOperationException(),
                    ThumbnailType.LogbookTeaser);
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

            
            PhotoAndThumbnailIdentification teaserIdentifiers = null;
            if (upsertModel.TeaserImage != null && upsertModel.TeaserImageContentType != null)
            {
                teaserIdentifiers = await photoService.AddPhotoAsync(
                    PhotoCategory.EventTeaser,
                    upsertModel.TeaserImage,
                    upsertModel.TeaserImageFileName,
                    upsertModel.TeaserImageContentType,
                    ThumbnailType.LogbookTeaser);
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
            [CanBeNull] PhotoAndThumbnailIdentification teaserIdentifiers)
        {
            if (upsertModel == null) throw new ArgumentNullException(nameof(upsertModel));
            if (logbookEntry == null) throw new ArgumentNullException(nameof(logbookEntry));
            
            logbookEntry.Title = upsertModel.Title;
            logbookEntry.TeaserText = upsertModel.Teaser ?? "";
            logbookEntry.Text = upsertModel.Text;
            logbookEntry.CreatedAt = upsertModel.CreatedAt;
            logbookEntry.ExternalPhotoAlbumUrl = upsertModel.ExternalPhotoAlbumUrl;
            logbookEntry.TeaserImage = teaserIdentifiers?.OriginalPhotoIdentifier?.ToString();
            logbookEntry.TeaserImageThumb = teaserIdentifiers?.ThumbnailPhotoIdentifier?.ToString();
        }
        
        private async Task RemoveTeaserImagesAsync([NotNull] LogbookEntry logbookEntry)
        {
            if (logbookEntry == null) throw new ArgumentNullException(nameof(logbookEntry));

            if (!string.IsNullOrWhiteSpace(logbookEntry.TeaserImage))
            {
                await photoService.RemovePhotosAsync(new PhotoIdentifier(PhotoCategory.EventTeaser, logbookEntry.TeaserImage));
            }

            if (!string.IsNullOrWhiteSpace(logbookEntry.TeaserImageThumb))
            {
                await photoService.RemovePhotosAsync(new PhotoIdentifier(PhotoCategory.EventTeaser, logbookEntry.TeaserImageThumb));
            }
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