using MediatR;
using Tauchbolde.SharedKernel.Services;

namespace Tauchbolde.SharedKernel
{
    public abstract class DomainEventBase : INotification
    {
        public DateTime DateOccurred { get; } = SystemClock.Now;
    }
}