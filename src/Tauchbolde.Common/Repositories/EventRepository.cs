using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tauchbolde.Common.Model;

namespace Tauchbolde.Common.Repositories
{
    public class EventRepository : RepositoryBase<Event>, IEventRepository
    {
        public EventRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        /// <inheritdoc />>
        public async Task<List<Event>> GetUpcommingEventsAsync()
        {
            return await CreateQueryForStartingAt(DateTime.Now)
                .Include(e => e.Comments)
                    .ThenInclude(c => c.Author)
                .Include(e => e.Participants)
                    .ThenInclude(p => p.User)
                .ToListAsync();
        }


        public async Task<ICollection<Event>> GetUpcomingAndRecentEventsAsync()
        {
            return await CreateQueryForStartingAt(DateTime.Now.AddDays(-30))
                .Include(e => e.Comments)
                    .ThenInclude(c => c.Author)
                .Include(e => e.Participants)
                    .ThenInclude(p => p.User)
                .ToListAsync();
        }

        public override async Task<Event> FindByIdAsync(Guid id)
        {
            return await context.Events
                .Include(e => e.Comments)
                .ThenInclude(c => c.Author)
                .Include(e => e.Participants)
                .ThenInclude(p => p.User)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public IQueryable<Event> CreateQueryForStartingAt(DateTime startDate, bool includeCanceled = false)
        {
            return context.Events
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
