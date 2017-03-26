using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Tauchbolde.Common.Model;

namespace Tauchbolde.Web.Models.EventViewModels
{
    public class EventEditViewModel
    {
        [Display(Name = "ID")]
        public Guid Id { get; set; }

        [Display(Name = "Name")]
        [Required]
        public string Name { get; set; }

        [Display(Name = "Organisator")]
        public string Organisator { get; set; }

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

        public IEnumerable<SelectListItem> BuddyTeamNames { get; set; }

        public Event OriginalEvent { get; set; }
    }
}
