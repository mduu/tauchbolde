using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Tauchbolde.Entities;
using Tauchbolde.Common.Repositories;
using Tauchbolde.UseCases.Logbook.DataAccess;

namespace Tauchbolde.DataAccess.Repositories
{
    /// <summary>
    /// Implements <see cref="ILogbookEntryRepository" /> and
    /// <see cref="ILogbookDataAccess"/> using EF Core.
    /// </summary>
    [UsedImplicitly]
    internal class LogbookEntryRepository : RepositoryBase<LogbookEntry>, ILogbookEntryRepository, ILogbookDataAccess
    {
        public LogbookEntryRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<LogbookEntry> FindByIdAsync(Guid id) =>
            await Context.LogbookEntries
                .Include(l => l.OriginalAuthor)
                .Include(l => l.EditorAuthor)
                .Include(l => l.Event)
                .FirstOrDefaultAsync(l => l.Id.Equals(id));
        
        /// <inheritdoc />
        public async Task<ICollection<LogbookEntry>> GetAllEntriesAsync(bool includeUnpublished) =>
            await Context.LogbookEntries
                .Where(l => includeUnpublished || l.IsPublished)
                .Include(l => l.OriginalAuthor)
                .Include(l => l.EditorAuthor)
                .Include(l => l.Event)
                .OrderByDescending(l => l.CreatedAt)
                .ToListAsync();

        /// <inheritdoc />
        public async Task<LogbookEntry> GetLogbookEntryByIdAsync(Guid logbookEntryId)
        {
            return await base.FindByIdAsync(logbookEntryId);
        }

        /// <inheritdoc />
        public async Task UpdateLogbookEntryAsync(LogbookEntry logbookEntry)
        {
            base.Update(logbookEntry);
        }

        public async Task SaveChangesAsync()
        {
            await Context.SaveChangesAsync();
        }
    }
}