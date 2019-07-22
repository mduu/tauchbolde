using System.Collections.Generic;
using Tauchbolde.Domain;
using Tauchbolde.Domain.Entities;

namespace Tauchbolde.Web.Models.EventViewModels
{
    public class EventListViewModel
    {
        public ICollection<Event> UpcommingEvents { get; set; }
    }
}
