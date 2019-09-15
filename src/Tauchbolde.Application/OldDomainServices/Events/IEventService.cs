using System;
using System.Threading.Tasks;
using Tauchbolde.Domain.Entities;

namespace Tauchbolde.Application.OldDomainServices.Events
{
    public interface IEventService
    {
        /// <summary>
        /// Get the event with all details by its ID.
        /// </summary>
        /// <param name="eventId">The <see cref="Event.Id"/> of the event </param>
        /// <returns>The event with all details by its ID.</returns>
        Task<Event> GetByIdAsync(Guid eventId);
    }
}