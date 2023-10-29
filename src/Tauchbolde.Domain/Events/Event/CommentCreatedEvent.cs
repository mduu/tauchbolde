using JetBrains.Annotations;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Domain.Events.Event
{
    public class CommentCreatedEvent : DomainEventBase
    {
        public CommentCreatedEvent(Guid eventId, Guid commentId, Guid authorId, DateTimeOffset time, [NotNull] string text)
        {
            if (string.IsNullOrWhiteSpace(text)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(text));
            
            EventId = eventId;
            CommentId = commentId;
            AuthorId = authorId;
            Text = text;
            Time = time;
        }

        public Guid EventId { get; }
        public Guid CommentId { get; }
        public Guid AuthorId { get; }
        public string Text { get; }
        public DateTimeOffset Time { get; }
    }
}