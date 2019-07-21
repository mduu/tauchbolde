using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Tauchbolde.Domain.Entities;
using Tauchbolde.Domain.ValueObjects;

namespace Tauchbolde.Application.OldDomainServices.Logbook
{
    /// <summary>
    /// Domain services for the logbook functionality.
    /// </summary>
    public interface ILogbookService
    {
        /// <summary>
        /// Gets the collection of all <see cref="LogbookEntry"/>.
        /// </summary>
        /// <param name="includeUnpublished"></param>
        Task<ICollection<LogbookEntry>> GetAllEntriesAsync(bool includeUnpublished = false);

        /// <summary>
        /// Gets the <see cref="LogbookEntry"/> by its ID.
        /// </summary>
        /// <param name="logbookEntryId">The ID of the logbook entry to get.</param>
        /// <returns>The <see cref="LogbookEntry"/>.</returns>
        Task<LogbookEntry> FindByIdAsync(Guid logbookEntryId);

        /// <summary>
        /// Update or insert an entry into the logbook.
        /// </summary>
        /// <param name="model">The logbook model to update or insert.</param>
        /// <returns>The ID of the logbook entry.</returns>
        Task<Guid> UpsertAsync(LogbookUpsertModel model);

        /// <summary>
        /// Deletes a <see cref="LogbookEntry"/> by its <see cref="LogbookEntry.Id"/>.
        /// </summary>
        /// <param name="logbookEntryId">The Id of the <see cref="LogbookEntry"/> to delete.</param>
        Task DeleteAsync(Guid logbookEntryId);

        /// <summary>
        /// Get a logbook photo by its identifier.
        /// </summary>
        /// <param name="photoIdentifier">The identifier of the photo to retrieve.</param>
        /// <returns>The photo data including metadata and binary stream.</returns>
        [NotNull] Task<Photo> GetPhotoDataAsync([NotNull] PhotoIdentifier photoIdentifier);
    }
}