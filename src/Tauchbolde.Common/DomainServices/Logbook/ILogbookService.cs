using System.Collections.Generic;
using System.Threading.Tasks;
using Tauchbolde.Common.Model;

namespace Tauchbolde.Common.DomainServices.Logbook
{
    /// <summary>
    /// Domain services for the logbook functionality.
    /// </summary>
    public interface ILogbookService
    {
        /// <summary>
        /// Gets the collection of all <see cref="LogbookEntry"/>.
        /// </summary>
        Task<ICollection<LogbookEntry>> GetAllEntriesAsync();
    }
}