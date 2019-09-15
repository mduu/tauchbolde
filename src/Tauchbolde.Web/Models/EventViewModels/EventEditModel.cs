using System;
using System.ComponentModel.DataAnnotations;

namespace Tauchbolde.Web.Models.EventViewModels
{
    public class EventEditModel
    {
        [Display(Name = "Name")]
        [Required]
        public string Name { get; set; }

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
    }
}