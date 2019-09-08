using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tauchbolde.Domain.Entities;

namespace Tauchbolde.Application.OldDomainServices.Events
{
    public interface IEventService
    {
        /// <summary>
        /// Insert or update the given <paramref name="eventToUpsert"/>.
        /// </summary>
        /// <param name="eventToUpsert">Eventdata to insert or update.</param>
        /// <param name="currentUser">The current users diver record.</param>
        /// <returns>The inserted or updated <see cref="Event"/>.</returns>
        Task<Event> UpsertEventAsync(Event eventToUpsert, Diver currentUser);

        /// <summary>
        /// Gets upcoming and recent events.
        /// </summary>
        /// <returns>Upcoming and recent events.</returns>
        Task<ICollection<Event>> GetUpcomingAndRecentEventsAsync();

        /// <summary>
        /// Gets upcoming events.
        /// </summary>
        /// <returns>Upcoming events.</returns>
        Task<ICollection<Event>> GetUpcomingEventsAsync();

        /// <summary>
        /// Get the event with all details by its ID.
        /// </summary>
        /// <param name="eventId">The <see cref="Event.Id"/> of the event </param>
        /// <returns>The event with all details by its ID.</returns>
        Task<Event> GetByIdAsync(Guid eventId);
    }
}