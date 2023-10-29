using Tauchbolde.SharedKernel;

namespace Tauchbolde.Domain.Events.Event
{
    public class CommentDeletedEvent : DomainEventBase
    {
        public CommentDeletedEvent(Guid commentId)
        {
            CommentId = commentId;
        }
        
        public Guid CommentId { get; }
    }
}