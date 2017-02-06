using System.Collections.Generic;
using Tauchbolde.Common.Model;

namespace Tauchbolde.Web.Models.EventViewModels
{
    public class EventListViewModel
    {
        public ICollection<Event> UpcommingEvents { get; set; }
    }
}
