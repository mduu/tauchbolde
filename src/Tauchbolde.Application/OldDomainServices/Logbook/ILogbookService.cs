using System;
using System.Threading.Tasks;
using Tauchbolde.Domain.Entities;

namespace Tauchbolde.Application.OldDomainServices.Logbook
{
    /// <summary>
    /// Domain services for the logbook functionality.
    /// </summary>
    public interface ILogbookService
    {
        /// <summary>
        /// Gets the <see cref="LogbookEntry"/> by its ID.
        /// </summary>
        /// <param name="logbookEntryId">The ID of the logbook entry to get.</param>
        /// <returns>The <see cref="LogbookEntry"/>.</returns>
        Task<LogbookEntry> FindByIdAsync(Guid logbookEntryId);
    }
}