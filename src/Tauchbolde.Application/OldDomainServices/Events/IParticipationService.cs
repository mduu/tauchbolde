using System;
using System.Threading.Tasks;
using Tauchbolde.Domain.Entities;
using Tauchbolde.Domain.Types;

namespace Tauchbolde.Application.OldDomainServices.Events
{
    public interface IParticipationService
    {
        /// <summary>
        /// Changes the participation for the given <paramref name="user"/> in event with <paramref name="eventId"/>.
        /// </summary>
        Task<Participant> ChangeParticipationAsync(
            Diver user,
            Guid eventId,
            ParticipantStatus status,
            int numberOfPeople,
            string note,
            string buddyTeamName);

        /// <summary>
        /// Gets the existing participation status for a user and event.
        /// </summary>
        /// <param name="user">ID of the User to get the participation status for.</param>
        /// <param name="eventId">Id of the event to get the participation status for.</param>
        /// <return>The existing <see cref="Participant"/> object for the user / event.</return>
        Task<Participant> GetExistingParticipationAsync(
            Diver user,
            Guid eventId);
    }
}