using System.Collections.Generic;
using System.Threading.Tasks;
using Tauchbolde.Common.Model;

namespace Tauchbolde.Common.Repositories
{
    /// <summary>
    /// Defines data-access for <see cref="LogbookEntry"/>.
    /// </summary>
    public interface ILogbookEntryRepository : IRepository<LogbookEntry>
    {
        /// <summary>
        /// Gets all <see cref="LogbookEntry"/> without any filter.
        /// </summary>
        /// <param name="includeUnpublished"></param>
        /// <returns>All <see cref="LogbookEntry"/>.</returns>
        Task<ICollection<LogbookEntry>> GetAllEntriesAsync(bool includeUnpublished);
    }
}