using System;
using MediatR;

namespace Tauchbolde.SharedKernel
{
    public abstract class DomainEventBase : INotification
    {
        public DateTime DateOccurred { get; protected set; } = DateTime.UtcNow;
    }
}