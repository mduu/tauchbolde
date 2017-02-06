using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tauchbolde.Common.Model
{
    public class Event
    {
        [Display(Name = "ID")]
        public Guid Id { get; set; }

        [Display(Name = "Name")]
        [Required]
        public string Name { get; set; }

        [Display(Name = "Organisator")]
        [Required]
        public ApplicationUser Organisator { get; set; }

        [Display(Name = "Ort / TP")]
        public string Location { get; set; }

        [Display(Name = "Treffpunkt")]
        public string MeetingPoint { get; set; }

        [Display(Name = "Beschreibung")]
        public string Description { get; set; }

        [Display(Name = "Startzeit")]
        [Required]
        public DateTime StartTime { get; set; }

        [Display(Name = "Endzeit")]
        public DateTime? EndTime { get; set; }

        [Display(Name ="Abgesagt")]
        [Required]
        public bool Canceled { get; set; }

        [Display(Name ="Gelöscht")]
        [Required]
        public bool Deleted { get; set; }

        public virtual ICollection<Participant> Participants { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }

        [NotMapped]
        public string StartEndTimeAsString
        {
            get
            {
                return EndTime != null
                    ? StartTime.Date == EndTime.Value.Date 
                        ? $"{StartTime:g} - {EndTime.Value:t}"
                        : $"{StartTime:g} - {EndTime.Value:g}"
                    : $"{StartTime:g}";
            }
        }
    }
}
