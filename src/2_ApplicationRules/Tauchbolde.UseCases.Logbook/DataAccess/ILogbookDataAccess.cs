using System;
using System.Threading.Tasks;
using Tauchbolde.Entities;

namespace Tauchbolde.UseCases.Logbook.DataAccess
{
    /// <summary>
    /// Defines Logbook related data-access.
    /// </summary>
    public interface ILogbookDataAccess
    {
        Task<LogbookEntry> GetLogbookEntryByIdAsync(Guid logbookEntryId);
        Task UpdateLogbookEntryAsync(LogbookEntry logbookEntry);
        Task SaveChangesAsync();
    }
}