using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;
using Tauchbolde.SharedKernel;
using Tauchbolde.SharedKernel.Extensions;

namespace Tauchbolde.Domain.Entities
{
    // TODO Change setters and ctor() to internal
    public class Event : EntityBase
    {
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

        [Display(Name ="Abgesagt")]
        [Required]
        public bool Canceled { get; [UsedImplicitly] set; }

        [Display(Name ="Gelöscht")]
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
    }
}
