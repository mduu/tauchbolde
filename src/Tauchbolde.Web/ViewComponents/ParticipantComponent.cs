using Microsoft.AspNetCore.Mvc;
using Tauchbolde.InterfaceAdapters.MVC.Presenters.Events.Details;

namespace Tauchbolde.Web.ViewComponents
{
    public class ParticipantComponent: ViewComponent
    {
        public IViewComponentResult Invoke(EventParticipantViewModel participant)
        {
            return View(participant);
        }
    }
}
