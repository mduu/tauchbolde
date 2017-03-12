using System;
using System.Threading.Tasks;
using Tauchbolde.Common.Model;

namespace Tauchbolde.Common.DomainServices
{
    public interface IParticipationService
    {
        /// <summary>
        /// Changes the participation for the given <paramref name="user"/> in event with <paramref name="eventId"/>.
        /// </summary>
        Task<Participant> ChangeParticipationAsync(ApplicationUser user, Guid eventId, ParticipantStatus status, int numberOfPeople, string note, string buddyTeamName);

        /// <inheritdoc />
        Task<Participant> GetExistingParticipationAsync(ApplicationUser user, Guid eventId);
    }
}