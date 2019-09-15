using System;
using System.Threading.Tasks;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.Domain.Entities;

namespace Tauchbolde.Application.OldDomainServices.Events
{
    internal class EventService : IEventService
    {
        private readonly IEventRepository eventRepository;

        public EventService(IEventRepository eventRepository)
        {
            this.eventRepository = eventRepository ?? throw new ArgumentNullException(nameof(eventRepository));
        }
        
        public async Task<Event> GetByIdAsync(Guid eventId) => await eventRepository.FindByIdAsync(eventId);
    }
}
