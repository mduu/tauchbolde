using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
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
        [NotNull] private readonly IDiverRepository diverRepository;
        [NotNull] private readonly ITelemetryService telemetryService;

        public LogbookService(
            [NotNull] ILogbookEntryRepository logbookEntryRepository,
            [NotNull] IDiverService diverService,
            [NotNull] IDiverRepository diverRepository,
            [NotNull] ITelemetryService telemetryService)
        {
            this.logbookEntryRepository = logbookEntryRepository ?? throw new ArgumentNullException(nameof(logbookEntryRepository));
            this.diverService = diverService ?? throw new ArgumentNullException(nameof(diverService));
            this.diverRepository = diverRepository ?? throw new ArgumentNullException(nameof(diverRepository));
            this.telemetryService = telemetryService ?? throw new ArgumentNullException(nameof(telemetryService));
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

            MapUpsertModelToLogbookEntry(upsertModel, existingLogbookEntry);
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
            
            MapUpsertModelToLogbookEntry(upsertModel, newLogbookEntry);

            await logbookEntryRepository.InsertAsync(newLogbookEntry);
            
            TrackLogbookEntry("LOGBOOK-UPDATE", newLogbookEntry);

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
        
        private static void MapUpsertModelToLogbookEntry(LogbookUpsertModel upsertModel, LogbookEntry existingLogbookEntry)
        {
            existingLogbookEntry.Title = upsertModel.Title;
            existingLogbookEntry.TeaserText = upsertModel.Teaser ?? "";
            existingLogbookEntry.Text = upsertModel.Text;
            existingLogbookEntry.CreatedAt = upsertModel.CreatedAt;
            existingLogbookEntry.ExternalPhotoAlbumUrl = upsertModel.ExternalPhotoAlbumUrl;
        }

        private async Task<Diver> GetCurrentUserAsync(LogbookUpsertModel model)
        {
            var currentUser = await diverService.GetMemberAsync(diverRepository, model.CurrentDiverId);
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