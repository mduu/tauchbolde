using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Tauchbolde.Common.Domain.Repositories;
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

        public override async Task<LogbookEntry> FindByIdAsync(Guid id)
        {
            return await Context.LogbookEntries
                .Include(l => l.OriginalAuthor)
                .Include(l => l.EditorAuthor)
                .Include(l => l.Event)
                .FirstOrDefaultAsync(l => l.Id.Equals(id));
        }


        /// <inheritdoc />
        public async Task<ICollection<LogbookEntry>> GetAllEntriesAsync()
        {
            return await Context.LogbookEntries
                .Include(l => l.OriginalAuthor)
                .Include(l => l.EditorAuthor)
                .Include(l => l.Event)
                .OrderByDescending(l => l.CreatedAt)
                .ToListAsync();        
        }
    }
}