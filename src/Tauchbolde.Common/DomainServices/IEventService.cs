using System;
using System.IO;
using System.Threading.Tasks;
using Tauchbolde.Common.Model;
using Tauchbolde.Common.Repositories;

namespace Tauchbolde.Common.DomainServices
{
    public interface IEventService
    {
        Task<Stream> CreateIcalForEvent(Guid eventId, IEventRepository eventRepository);

        /// <summary>
        /// Updates an existing event with new values.
        /// </summary>
        /// <returns>The updated event.</returns>
        Task<Event> UpdateEventAsync(IEventRepository eventRepository, Guid eventId, string name,
            string description, DateTime startTime, DateTime? endTime, string location,
            string meetingPoint, ApplicationUser currentUser);

        /// <summary>
        /// Insert or update the given <paramref name="eventToUpsert"/>.
        /// </summary>
        /// <param name="eventRepository"></param>
        /// <param name="eventToUpsert">Eventdata to insert or update.</param>
        /// <returns>The inserted or updated <see cref="Event"/>.</returns>
        Task<Event> UpsertEventAsync(IEventRepository eventRepository, Event eventToUpsert);
    }
}