using Tauchbolde.SharedKernel;

namespace Tauchbolde.Domain.Events.Diver
{
    public class UserProfileEditedEvent : DomainEventBase
    {
        public Guid EditedDiverId { get; }
        public Guid EditedByDiverId { get; }

        public UserProfileEditedEvent(Guid editedDiverId, Guid editedByDiverId)
        {
            EditedDiverId = editedDiverId;
            EditedByDiverId = editedByDiverId;
        }
    }
}