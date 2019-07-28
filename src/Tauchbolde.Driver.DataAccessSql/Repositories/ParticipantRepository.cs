using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.Domain.Entities;
using Tauchbolde.Domain.Types;
using Tauchbolde.Driver.DataAccessSql.DataEntities;
using Tauchbolde.Driver.DataAccessSql.Mappers;

namespace Tauchbolde.Driver.DataAccessSql.Repositories
{
    internal class ParticipantRepository : RepositoryBase<Participant, ParticipantData>, IParticipantRepository
    {
        public ParticipantRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Participant> GetParticipantByIdAsync(Guid participationId) =>
            (await Context.Participants
                .Include(p => p.Event)
                .Include(p => p.ParticipatingDiver)
                .FirstOrDefaultAsync(p => p.Id == participationId))
            .MapTo();


        public async Task<Participant> GetParticipationForEventAndUserAsync(Diver user, Guid eventId) =>
            (await Context.Participants
                .Include(p => p.Event)
                .Include(p => p.ParticipatingDiver)
                .FirstOrDefaultAsync(p =>
                    p.EventId == eventId && p.ParticipatingDiver.Id == user.Id))
            .MapTo();

        public async Task<ICollection<Participant>> GetParticipantsForEventByStatusAsync(Guid eventId,
            ParticipantStatus status) =>
            (await Context.Participants
                .Include(p => p.Event)
                .Include(p => p.ParticipatingDiver)
                .Where(p => p.EventId == eventId && p.Status == status)
                .ToListAsync())
            .Select(p => p.MapTo())
            .ToList();
                 
        protected override Participant MapTo(ParticipantData dataEntity) => dataEntity.MapTo();
        protected override ParticipantData MapTo(Participant domainEntity) => domainEntity.MapTo();
    }
}