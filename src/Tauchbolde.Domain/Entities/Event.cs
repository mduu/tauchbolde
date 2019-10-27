using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Tauchbolde.Domain.Events.Event;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Domain.Entities
{
    public class Event : EntityBase
    {
        public Event(
            [NotNull] string name,
            [NotNull] string description,
            [NotNull] string location,
            [NotNull] string meetingPoint,
            DateTime startTime,
            DateTime? endTime,
            Guid organisatorId)
        {
            Id = Guid.NewGuid();
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description ?? throw new ArgumentNullException(nameof(description));
            Location = location ?? throw new ArgumentNullException(nameof(location));
            MeetingPoint = meetingPoint ?? throw new ArgumentNullException(nameof(meetingPoint));
            StartTime = startTime;
            EndTime = endTime;
            OrganisatorId = organisatorId;

            RaiseDomainEvent(new EventCreatedEvent(Id));
        }

        internal Event()
        {
        }
        
        [Required] public string Name { get; [UsedImplicitly] internal set; }
        [Required] public Guid OrganisatorId { get; [UsedImplicitly] internal set; }
        [Required] public Diver Organisator { get; [UsedImplicitly] internal set; }
        public string Location { get; [UsedImplicitly] internal set; }
        public string MeetingPoint { get; [UsedImplicitly] internal set; }
        public string Description { get; [UsedImplicitly] internal set; }
        [Required] public DateTime StartTime { get; [UsedImplicitly] internal set; }
        public DateTime? EndTime { get; [UsedImplicitly] internal set; }
        [Required] public bool Canceled { get; [UsedImplicitly] internal set; }
        [Required] public bool Deleted { get; [UsedImplicitly] internal set; }

        public virtual ICollection<Participant> Participants { get; [UsedImplicitly] internal set; }
        public virtual ICollection<Comment> Comments { get; [UsedImplicitly] internal set; } = new List<Comment>();

        public Comment AddNewComment(Guid authorId, [NotNull] string text)
        {
            if (authorId == Guid.Empty) throw new ArgumentException("Guid.Empty is not allowed!", nameof(authorId));
            if (string.IsNullOrWhiteSpace(text)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(text));

            var newComment = new Comment(Id, authorId, text);
            Comments.Add(newComment);

            return newComment;
        }

        public bool Edit(
            Guid currentDiverId,
            [NotNull] string title,
            [NotNull] string description,
            [NotNull] string location,
            [NotNull] string meetingPoint,
            DateTime startTime,
            DateTime? endTime)
        {
            var logger = new Logger<Event>(new LoggerFactory());

            if (currentDiverId != OrganisatorId)
            {
                logger.LogError("Edit event is only allowed for organizers! User [{userId}] is not the organizer of event [{eventId}]!", currentDiverId, Id);
                return false;
            }

            Name = title ?? throw new ArgumentNullException(nameof(title));
            Description = description ?? throw new ArgumentNullException(nameof(description));
            Location = location ?? throw new ArgumentNullException(nameof(location));
            MeetingPoint = meetingPoint ?? throw new ArgumentNullException(nameof(meetingPoint));
            StartTime = startTime;
            EndTime = endTime;

            RaiseDomainEvent(new EventEditedEvent(Id));

            return true;
        }
    }
}