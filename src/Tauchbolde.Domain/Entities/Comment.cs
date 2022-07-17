using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;
using Tauchbolde.Domain.Events.Event;
using Tauchbolde.SharedKernel;
using Tauchbolde.SharedKernel.Services;

namespace Tauchbolde.Domain.Entities
{
    public class Comment : EntityBase
    {
        internal Comment()
        {
        }

        public Comment(Guid eventId, Guid authorId, [NotNull] string text)
        {
            if (eventId == Guid.Empty) throw new ArgumentException("Guid.Empty not allowed!", nameof(eventId));
            if (authorId == Guid.Empty) throw new ArgumentException("Guid.Empty not allowed!", nameof(authorId));
            if (string.IsNullOrWhiteSpace(text)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(text));

            Id = Guid.NewGuid();
            EventId = eventId;
            AuthorId = authorId;
            Text = text;
            CreateDate = SystemClock.Now;
            
            RaiseDomainEvent(new CommentCreatedEvent(EventId, Id, authorId, DateTime.UtcNow, text));
        }
        
        [Display(Name = "Anlass ID")]
        public Guid EventId { get; internal set; }
        public Event Event { get; [UsedImplicitly] internal set; }
        
        [Display(Name = "Autor ID")]
        [Required]
        public Guid AuthorId { get; internal set; }

        [Display(Name = "Autor")]
        [Required]
        public Diver Author { get; [UsedImplicitly] internal set; }

        [Display(Name = "Geschrieben um")]
        [Required]
        public DateTime CreateDate { get; internal set; }

        [Display(Name = "Geändert um")]
        public DateTime? ModifiedDate { get; private set; }

        [Display(Name = "Kommentar")]
        [Required]
        public string Text { get; internal set; }

        public void Edit(string text)
        {
            Text = text;
            ModifiedDate = SystemClock.Now;
            RaiseDomainEvent(new CommentEditedEvent(Id, EventId, AuthorId, DateTime.Now, Text));
        }

        public void Delete()
        {
            RaiseDomainEvent(new CommentDeletedEvent(Id));
        }
    }
}
