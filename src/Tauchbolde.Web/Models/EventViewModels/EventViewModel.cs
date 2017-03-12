using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Tauchbolde.Common.Model;

namespace Tauchbolde.Web.Models.EventViewModels
{
    public class EventViewModel
    {
        public Event Event { get; set; }

        public IEnumerable<SelectListItem> BuddyTeamNames { get; set; }

        public ChangeParticipantViewModel ChangeParticipantViewModel { get; set; }
    }
}
