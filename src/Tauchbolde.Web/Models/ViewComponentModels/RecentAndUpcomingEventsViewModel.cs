using System.Collections.Generic;
using Tauchbolde.Domain;
using Tauchbolde.Domain.Entities;

namespace Tauchbolde.Web.Models.ViewComponentModels
{
    public class RecentAndUpcomingEventsViewModel
    {
        public IList<Event> RecentEvents { get; set; }
    }
}
