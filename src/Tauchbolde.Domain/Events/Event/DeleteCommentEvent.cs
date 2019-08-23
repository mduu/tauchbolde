using System;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Domain.Events.Event
{
    public class DeleteCommentEvent : DomainEventBase
    {
        public DeleteCommentEvent(Guid commentId)
        {
            CommentId = commentId;
        }
        
        public Guid CommentId { get; }
    }
}