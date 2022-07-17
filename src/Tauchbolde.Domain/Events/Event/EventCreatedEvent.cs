using Tauchbolde.SharedKernel;

namespace Tauchbolde.Domain.Events.Event
{
    public class EventCreatedEvent : DomainEventBase
    {
        public EventCreatedEvent(Guid eventId)
        {
            EventId = eventId;
        }

        public Guid EventId { get; }
    }
}