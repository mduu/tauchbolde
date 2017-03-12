using System;
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

        public async Task<Participant> GetParticipationForEventAndUserAsync(ApplicationUser user, Guid eventId)
        {
            return await context.Participants.FirstOrDefaultAsync(p =>
                p.EventId == eventId && p.User.Id == user.Id);
        }
    }
}
