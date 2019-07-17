using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Tauchbolde.Entities;

namespace Tauchbolde.Web.Models.EventViewModels
{
    public class EventViewModel
    {
        public Event Event { get; set; }
        public Diver CurrentDiver { get; internal set; }

        public IEnumerable<SelectListItem> BuddyTeamNames { get; set; }

        public ChangeParticipantViewModel ChangeParticipantViewModel { get; set; }
        public bool AllowEdit { get; set; }
    }
}
