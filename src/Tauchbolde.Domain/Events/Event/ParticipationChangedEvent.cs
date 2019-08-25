using System;
using JetBrains.Annotations;
using Tauchbolde.Domain.Types;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Domain.Events.Event
{
    public class ParticipationChangedEvent : DomainEventBase
    {
        public ParticipationChangedEvent(Guid participationId, Guid eventId, Guid diverId, ParticipantStatus status)
        {
            ParticipationId = participationId;
            EventId = eventId;
            DiverId = diverId;
            Status = status;
        }

        public Guid ParticipationId { [UsedImplicitly] get; }
        public Guid EventId { [UsedImplicitly] get; }
        public Guid DiverId { [UsedImplicitly] get; }
        public ParticipantStatus Status { [UsedImplicitly] get; }
    }
}