using Microsoft.EntityFrameworkCore;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.Domain.Entities;
using Tauchbolde.Domain.Types;

namespace Tauchbolde.Driver.DataAccessSql.Repositories
{
    internal class ParticipantRepository : RepositoryBase<Participant>, IParticipantRepository
    {
        public ParticipantRepository(ApplicationDbContext context) : base(context)
        {
        }
        
        public async Task<Participant> GetParticipantByIdAsync(Guid participationId) =>
            await Context.Participants
                .Include(p => p.Event)
                .Include(p => p.ParticipatingDiver)
                .FirstOrDefaultAsync(p => p.Id == participationId);


        public async Task<Participant> GetParticipationForEventAndUserAsync(Diver user, Guid eventId) =>
            await Context.Participants
                .Include(p => p.Event)
                .Include(p => p.ParticipatingDiver)
                .FirstOrDefaultAsync(p =>
                    p.EventId == eventId && p.ParticipatingDiver.Id == user.Id);

        public async Task<ICollection<Participant>> GetParticipantsForEventByStatusAsync(Guid eventId, ParticipantStatus status) =>
            await Context.Participants
                .Include(p => p.Event)
                .Include(p => p.ParticipatingDiver)
                .Where(p => p.EventId == eventId && p.Status == status).ToListAsync();
    }
}
