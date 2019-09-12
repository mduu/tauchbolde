using System;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Domain.Events.Event
{
    public class EventEditedEvent : DomainEventBase
    {
        public EventEditedEvent(Guid eventId)
        {
            EventId = eventId;
        }

        public Guid EventId { get; }
    }
}