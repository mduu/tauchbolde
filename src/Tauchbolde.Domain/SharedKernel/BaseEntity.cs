using System;
using System.Collections.Generic;

namespace Tauchbolde.Domain.SharedKernel
{
    public abstract class BaseEntity : IEntity
    {
        public Guid Id { get; set; }
        public IList<BaseDomainEvent> Events { get; } = new List<BaseDomainEvent>();
    }
}