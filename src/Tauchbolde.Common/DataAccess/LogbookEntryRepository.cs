using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Tauchbolde.Common.DomainServices.Repositories;
using Tauchbolde.Common.Model;

namespace Tauchbolde.Common.DataAccess
{
    /// <summary>
    /// Implements <see cref="ILogbookEntryRepository" /> using EF Core.
    /// </summary>
    [UsedImplicitly]
    internal class LogbookEntryRepository : RepositoryBase<LogbookEntry>, ILogbookEntryRepository
    {
        public LogbookEntryRepository(ApplicationDbContext context) : base(context)
        {
        }

        /// <inheritdoc />
        public async Task<ICollection<LogbookEntry>> GetAllEntriesAsync()
        {
            return await Context.LogbookEntries
                .ToListAsync();        
        }
    }
}