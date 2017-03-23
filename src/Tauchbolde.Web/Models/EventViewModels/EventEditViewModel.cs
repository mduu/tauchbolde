using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Tauchbolde.Common.Model;

namespace Tauchbolde.Web.Models.EventViewModels
{
    public class EventEditViewModel
    {
        public Event Event { get; set; }

        public IEnumerable<SelectListItem> BuddyTeamNames { get; set; }
    }
}
