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
            return await context.Events
                .Where(e =>
                    e.Deleted == false &&
                    e.Canceled == false &&
                    e.EndTime > DateTime.Now)
                .OrderBy(e => e.StartTime)
                .ThenBy(e => e.EndTime)
                .Include(e => e.Comments)
                    .ThenInclude(c => c.Author)
                .Include(e => e.Participants)
                    .ThenInclude(p => p.User)
                .ToListAsync();
        }

        public override async Task<Event> GetById(Guid id)
        {
            return await context.Events
                .Include(e => e.Comments)
                .ThenInclude(c => c.Author)
                .Include(e => e.Participants)
                .ThenInclude(p => p.User)
                .FirstOrDefaultAsync(e => e.Id == id);
        }
    }
}
