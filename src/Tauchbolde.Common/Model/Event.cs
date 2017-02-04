using System;
using System.ComponentModel.DataAnnotations;

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
        public string Organisator { get; set; }

        [Display(Name = "Ort / TP")]
        public string Location { get; set; }

        [Display(Name = "Treffpunkt")]
        public string MeetingPoint { get; set; }

        [Display(Name = "Beschreibung")]
        public string Description { get; set; }

        [Display(Name = "Startzeit")]
        public DateTime StartTime { get; set; }

        [Display(Name = "Endzeit")]
        public DateTime EndTime { get; set; }
    }
}
