using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tauchbolde.Common.Model;
using Tauchbolde.Common.Repository;

namespace Tauchbolde.Common.Repositories
{
    public interface IParticipantRepository : IRepository<Participant>
    {
        Task<Participant> GetParticipationForEventAndUserAsync(ApplicationUser user, Guid eventId);
        Task<ICollection<Participant>> GetParticipantsForEventByStatusAsync(Guid eventId, ParticipantStatus? status);
    }
}
