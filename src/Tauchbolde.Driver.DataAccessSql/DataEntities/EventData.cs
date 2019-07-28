using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tauchbolde.Domain.Helpers;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Driver.DataAccessSql.DataEntities
{
    [Table("Events")]
    public class EventData : EntityBase
    {
        [Display(Name = "Name")]
        [Required]
        public string Name { get; set; }

        [Display(Name = "Organisator Id")]
        [Required]
        public Guid OrganisatorId { get; set; }

        [Display(Name = "Organisator")]
        [Required]
        public DiverData Organisator { get; set; }

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

        public virtual ICollection<ParticipantData> Participants { get; set; }
        public virtual ICollection<CommentData> Comments { get; set; }

        [Display(Name = "Datum / Zeit")]
        [NotMapped]
        public string StartEndTimeAsString => EndTime != null
            ? StartTime.Date == EndTime.Value.Date
                ? $"{StartTime.ToStringSwissDate()} - {EndTime.Value.ToStringSwissTime()}"
                : $"{StartTime.ToStringSwissDate()} - {EndTime.ToStringSwissDate()}"
            : $"{StartTime.ToStringSwissDateTime()}";
    }
}
