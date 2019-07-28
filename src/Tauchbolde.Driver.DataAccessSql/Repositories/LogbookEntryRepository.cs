using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.Domain.Entities;
using Tauchbolde.Driver.DataAccessSql.DataEntities;
using Tauchbolde.Driver.DataAccessSql.Mappers;

namespace Tauchbolde.Driver.DataAccessSql.Repositories
{
    [UsedImplicitly]
    internal class LogbookEntryRepository : RepositoryBase<LogbookEntry, LogbookEntryData>, ILogbookEntryRepository
    {
        public LogbookEntryRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<LogbookEntry> FindByIdAsync(Guid id) =>
            (await Context.LogbookEntries
                .Include(l => l.OriginalAuthor)
                .Include(l => l.EditorAuthor)
                .Include(l => l.Event)
                .FirstOrDefaultAsync(l => l.Id.Equals(id)))
            .MapTo();

        public async Task<ICollection<LogbookEntry>> GetAllEntriesAsync(bool includeUnpublished) =>
            (await Context.LogbookEntries
                .Where(l => includeUnpublished || l.IsPublished)
                .Include(l => l.OriginalAuthor)
                .Include(l => l.EditorAuthor)
                .Include(l => l.Event)
                .OrderByDescending(l => l.CreatedAt)
                .ToListAsync())
            .Select(l => l.MapTo())
            .ToList();
           
        protected override LogbookEntry MapTo(LogbookEntryData dataEntity) => dataEntity.MapTo();
        protected override LogbookEntryData MapTo(LogbookEntry domainEntity) => domainEntity.MapTo();
    }
}