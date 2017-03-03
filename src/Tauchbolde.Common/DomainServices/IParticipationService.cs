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
        Task ChangeParticipationAsync( ApplicationUser user, Guid? existingParticipantId, Guid eventId, ParticipantStatus status, int numberOfPeople, string note, string buddyTeamName);
    }
}