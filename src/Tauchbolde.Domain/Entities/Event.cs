using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;
using Tauchbolde.Domain.Helpers;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Domain.Entities
{
    // TODO Change setters and ctor() to internal
    public class Event : EntityBase
    {
        [Display(Name = "Name")]
        [Required]
        public string Name { get; set; }

        [Display(Name = "Organisator Id")]
        [Required]
        public Guid OrganisatorId { get; set; }

        [Display(Name = "Organisator")]
        [Required]
        public Diver Organisator { get; set; }

        [Display(Name = "Ort / TP")]
        public string Location { get; set; }

        [Display(Name = "Treffpunkt")]
        public string MeetingPoint { get; set; }

        [Display(Name = "Beschreibung")]
        public string Description { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Startzeit")]
        [Required]
        public DateTime StartTime { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Endzeit")]
        public DateTime? EndTime { get; set; }

        [Display(Name ="Abgesagt")]
        [Required]
        public bool Canceled { get; set; }

        [Display(Name ="Gelöscht")]
        [Required]
        public bool Deleted { get; set; }

        public virtual ICollection<Participant> Participants { get; set; }
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

        [Display(Name = "Datum / Zeit")]
        [NotMapped]
        public string StartEndTimeAsString => EndTime != null
            ? StartTime.Date == EndTime.Value.Date
                ? $"{StartTime.ToStringSwissDate()} - {EndTime.Value.ToStringSwissTime()}"
                : $"{StartTime.ToStringSwissDate()} - {EndTime.ToStringSwissDate()}"
            : $"{StartTime.ToStringSwissDateTime()}";

        public Comment AddNewComment(Guid authorId, [NotNull] string text)
        {
            if (authorId == Guid.Empty) throw new ArgumentException("Guid.Empty is not allowed!", nameof(authorId));
            if (string.IsNullOrWhiteSpace(text)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(text));

            var newComment = new Comment(Id, authorId, text);
            Comments.Add(newComment);

            return newComment;
        }
    }
}
