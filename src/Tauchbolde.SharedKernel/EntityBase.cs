using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;

namespace Tauchbolde.SharedKernel
{
    public abstract class EntityBase : IEntity
    {
        [Key]
        public Guid Id { get; set; }
        
        [NotMapped]
        public IList<DomainEventBase> UncommittedDomainEvents { get; } = new List<DomainEventBase>();

        protected void RaiseDomainEvent([NotNull] DomainEventBase domainEvent)
        {
            if (domainEvent == null) throw new ArgumentNullException(nameof(domainEvent));
            
            UncommittedDomainEvents.Add(domainEvent);
        }
    }
}