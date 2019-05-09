using System.Collections.Generic;
using System.Threading.Tasks;
using Tauchbolde.Common.Model;

namespace Tauchbolde.Common.Domain.Repositories
{
    internal interface ILogbookEntryRepository : IRepository<LogbookEntry>
    {
        /// <summary>
        /// Gets all <see cref="LogbookEntry"/> without any filter.
        /// </summary>
        /// <returns></returns>
        Task<ICollection<LogbookEntry>> GetAllEntriesAsync();
    }
}