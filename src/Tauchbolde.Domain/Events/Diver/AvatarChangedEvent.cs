using System;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Domain.Events.Diver
{
    public class AvatarChangedEvent : DomainEventBase
    {
        public AvatarChangedEvent(Guid diverId)
        {
            DiverId = diverId;
        }

        public Guid DiverId { get; }
    }
}