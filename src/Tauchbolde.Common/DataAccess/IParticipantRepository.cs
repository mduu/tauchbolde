using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tauchbolde.Common.Model;

namespace Tauchbolde.Common.DataAccess
{
    public interface IParticipantRepository : IRepository<Participant>
    {
        Task<Participant> GetParticipationForEventAndUserAsync(Diver user, Guid eventId);
        Task<ICollection<Participant>> GetParticipantsForEventByStatusAsync(Guid eventId, ParticipantStatus status);
    }
}
