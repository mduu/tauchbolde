using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tauchbolde.Common.Model;

namespace Tauchbolde.Common.Repositories
{
    public interface IParticipantRepository : IRepository<Participant>
    {
        /// <summary>
        /// Gets the participant by <see cref="Participant.Id"/> including
        /// <see cref="Participant.Event"/> and <see cref="Participant.ParticipatingDiver"/> loaded async.
        /// </summary>
        /// <returns>The participant by ID.</returns>
        /// <param name="participationId">The <see cref="Participant.Id"/> to search for.</param>
        Task<Participant> GetParticipantByIdAsync(Guid participationId);
        
        /// <summary>
        /// Gets the participant by <paramref name="user"/> and <paramref name="eventId"/> including
        /// <see cref="Participant.Event"/> and <see cref="Participant.ParticipatingDiver"/> loaded async.
        /// </summary>
        /// <returns>The participation for event and user async.</returns>
        /// <param name="user">User to search for.</param>
        /// <param name="eventId">Event ID to search for.</param>
        Task<Participant> GetParticipationForEventAndUserAsync(Diver user, Guid eventId);
        
        /// <summary>
        /// Gets the collection of participants for an event and status async.
        /// </summary>
        /// <returns>The collection participants for an event by status async.</returns>
        /// <param name="eventId">Event ID to get the participants for.</param>
        /// <param name="status">Status the status to get the participants for.</param>
        Task<ICollection<Participant>> GetParticipantsForEventByStatusAsync(Guid eventId, ParticipantStatus status);
    }
}
