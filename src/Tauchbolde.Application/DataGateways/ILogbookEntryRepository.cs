using Tauchbolde.Domain.Entities;

namespace Tauchbolde.Application.DataGateways
{
    public interface ILogbookEntryRepository : IRepository<LogbookEntry>
    {
        Task<ICollection<LogbookEntry>> GetAllEntriesAsync(bool includeUnpublished);
    }
}