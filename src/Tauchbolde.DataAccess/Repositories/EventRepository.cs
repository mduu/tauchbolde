using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tauchbolde.Entities;
using Tauchbolde.Common.Repositories;

namespace Tauchbolde.DataAccess.Repositories
{
    internal class EventRepository : RepositoryBase<Event>, IEventRepository
    {
        public EventRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        /// <inheritdoc />>
        public async Task<List<Event>> GetUpcomingEventsAsync()
        {
            return await CreateQueryForStartingAt(DateTime.Now)
                .Include(e => e.Comments)
                    .ThenInclude(c => c.Author)
                .Include(e => e.Participants)
                    .ThenInclude(p => p.ParticipatingDiver)
                .ToListAsync();
        }


        public async Task<ICollection<Event>> GetUpcomingAndRecentEventsAsync()
        {
            return await CreateQueryForStartingAt(DateTime.Now.AddDays(-30))
                .Include(e => e.Comments)
                    .ThenInclude(c => c.Author)
                .Include(e => e.Participants)
                    .ThenInclude(p => p.ParticipatingDiver)
                .ToListAsync();
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
                .FirstOrDefaultAsync(e => e.Id == id);

            return result;
        }

        public IQueryable<Event> CreateQueryForStartingAt(DateTime startDate, bool includeCanceled = false)
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
    }
}
