using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tauchbolde.Common.Model;

namespace Tauchbolde.Common.Domain.Repositories
{
    /// <summary>
    /// Defines data-access for <see cref="LogbookEntry"/>.
    /// </summary>
    internal interface ILogbookEntryRepository : IRepository<LogbookEntry>
    {
//        /// <summary>
//        /// Finds and load a logbook entry with its sub-relations.
//        /// </summary>
//        /// <param name="id">The <see cref="LogbookEntry.Id"/> to find.</param>
//        /// <returns>A logbook entry with its sub-relations.</returns>
//        new Task<LogbookEntry> FindByIdAsync(Guid id);

        /// <summary>
        /// Gets all <see cref="LogbookEntry"/> without any filter.
        /// </summary>
        /// <param name="includeUnpublished"></param>
        /// <returns>All <see cref="LogbookEntry"/>.</returns>
        Task<ICollection<LogbookEntry>> GetAllEntriesAsync(bool includeUnpublished);
    }
}