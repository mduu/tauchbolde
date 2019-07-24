using System;
using System.Collections.Generic;

namespace Tauchbolde.SharedKernel
{
    public interface IEntity
    {
        Guid Id { get; set; }
        IList<DomainEventBase> UncommittedDomainEvents { get; }
    }
}