using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Tauchbolde.Domain.Events.Event;
using Tauchbolde.SharedKernel;
using Tauchbolde.SharedKernel.Extensions;

namespace Tauchbolde.Domain.Entities
{
    // TODO Change setters and ctor() to internal
    public class Event : EntityBase
    {
        public Event() // TODO Make internal
        {
        }
        
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

        [Display(Name = "Name")]
        [Required]
        public string Name { get; [UsedImplicitly] set; }

        [Display(Name = "Organisator Id")]
        [Required]
        public Guid OrganisatorId { get; [UsedImplicitly] set; }

        [Display(Name = "Organisator")]
        [Required]
        public Diver Organisator { get; [UsedImplicitly] set; }

        [Display(Name = "Ort / TP")]
        public string Location { get; [UsedImplicitly] set; }

        [Display(Name = "Treffpunkt")]
        public string MeetingPoint { get; [UsedImplicitly] set; }

        [Display(Name = "Beschreibung")]
        public string Description { get; [UsedImplicitly] set; }

        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Startzeit")]
        [Required]
        public DateTime StartTime { get; [UsedImplicitly] set; }

        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Endzeit")]
        public DateTime? EndTime { get; [UsedImplicitly] set; }

        [Display(Name = "Abgesagt")]
        [Required]
        public bool Canceled { get; [UsedImplicitly] set; }

        [Display(Name = "Gelöscht")]
        [Required]
        public bool Deleted { get; [UsedImplicitly] set; }

        public virtual ICollection<Participant> Participants { get; [UsedImplicitly] set; }
        public virtual ICollection<Comment> Comments { get; [UsedImplicitly] set; } = new List<Comment>();

        [Display(Name = "Datum / Zeit")]
        [NotMapped]
        [Obsolete]
        public string StartEndTimeAsString => StartTime.FormatTimeRange(EndTime);

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