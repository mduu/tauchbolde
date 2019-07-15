using System.Collections.Generic;
using Tauchbolde.Entities;

namespace Tauchbolde.Web.Models.ViewComponentModels
{
    public class RecentAndUpcomingEventsViewModel
    {
        public IList<Event> RecentEvents { get; set; }
    }
}
