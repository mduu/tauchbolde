using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.Application.Services.Core;
using Tauchbolde.Domain.Entities;
using Tauchbolde.Driver.DataAccessSql.DataEntities;
using Tauchbolde.Driver.DataAccessSql.Mappers;

namespace Tauchbolde.Driver.DataAccessSql.Repositories
{
    internal class EventRepository : RepositoryBase<Event, EventData>, IEventRepository
    {
        [NotNull] private readonly IClock clock;

        public EventRepository(
            [NotNull] ApplicationDbContext context,
            [NotNull] IClock clock)
            : base(context)
        {
            this.clock = clock ?? throw new ArgumentNullException(nameof(clock));
        }

        public async Task<List<Event>> GetUpcomingEventsAsync() =>
            (await CreateQueryForStartingAt(clock.Now().DateTime)
                .Include(e => e.Comments)
                .ThenInclude(c => c.Author)
                .Include(e => e.Participants)
                .ThenInclude(p => p.ParticipatingDiver)
                .ToListAsync())
            .Select(e => e.MapTo())
            .ToList();


        public async Task<ICollection<Event>> GetUpcomingAndRecentEventsAsync()
        {
            var events = await CreateQueryForStartingAt(clock.Now().AddDays(-30).DateTime)
                .Include(e => e.Comments)
                .ThenInclude(c => c.Author)
                .Include(e => e.Participants)
                .ThenInclude(p => p.ParticipatingDiver)
                .ToListAsync();
            
            return events
                .Select(e => e.MapTo())
                .ToList();
        }

        public override async Task<Event> FindByIdAsync(Guid id)
        {
            // HACK: The following magic line seems to enforce that the comments
            //       get loaded. If this line is not pressent the loading of the
            //       event does not load its comments even proper include statements
            //       are present! Don't get it why.
            var comments = await Context.Comments.Where(c => c.EventId == id).ToListAsync();

            var result = await Context.Events
                .Include(e => e.Comments)
                .ThenInclude(c => c.Author)
                .Include(e => e.Participants)
                .ThenInclude(p => p.ParticipatingDiver)
                .ThenInclude(d => d.User)
                .Include(e => e.Organisator)
                .FirstOrDefaultAsync(e => e.Id == id);

            return result.MapTo();
        }

        private IQueryable<EventData> CreateQueryForStartingAt(DateTime startDate, bool includeCanceled = false)
        {
            return Context.Events
                .Where(e =>
                    e.Deleted == false &&
                    (includeCanceled || e.Canceled == false) &&
                    (e.EndTime.HasValue && e.EndTime > startDate ||
                     !e.EndTime.HasValue && e.StartTime > startDate))
                .OrderBy(e => e.StartTime)
                .ThenBy(e => e.EndTime);
        }
        
        protected override Event MapTo(EventData dataEntity) => dataEntity.MapTo();
        protected override EventData MapTo(Event domainEntity) => domainEntity.MapTo();
    }
}