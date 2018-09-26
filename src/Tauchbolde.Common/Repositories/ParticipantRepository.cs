using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tauchbolde.Common.Model;

namespace Tauchbolde.Common.Repositories
{
    public class ParticipantRepository : RepositoryBase<Participant>, IParticipantRepository
    {
        public ParticipantRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Participant> GetParticipationForEventAndUserAsync(Diver user, Guid eventId)
        {
            return await Context.Participants.FirstOrDefaultAsync(p =>
                p.EventId == eventId && p.ParticipatingDiver.Id == user.Id);
        }

        public async Task<ICollection<Participant>> GetParticipantsForEventByStatusAsync(Guid eventId, ParticipantStatus? status)
        {
            if (eventId == Guid.Empty) throw new AggregateException($"{nameof(eventId)} must not be empty!");

            var query = Context.Participants
                .Include(p => p.Event)
                .Include(p => p.ParticipatingDiver)
                .Where(p => p.EventId == eventId);

            if (status.HasValue)
            {
                query.Where(p => p.Status == status.Value);
            }

            return await query.ToListAsync();
        }
    }
}
