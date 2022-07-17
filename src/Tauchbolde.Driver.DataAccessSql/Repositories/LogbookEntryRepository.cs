using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.Domain.Entities;

namespace Tauchbolde.Driver.DataAccessSql.Repositories
{
    [UsedImplicitly]
    internal class LogbookEntryRepository : RepositoryBase<LogbookEntry>, ILogbookEntryRepository
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
    }
}