using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tauchbolde.Common.DataAccess;
using Tauchbolde.Web.Models.ViewComponentModels;

namespace Tauchbolde.Web.ViewComponents
{
    public class RecentAndUpcomingEventsViewComponent : ViewComponent
    {
        private readonly IEventRepository eventRepository;

        public RecentAndUpcomingEventsViewComponent(IEventRepository eventRepository)
        {
            this.eventRepository = eventRepository ?? throw new ArgumentNullException(nameof(eventRepository));
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = new RecentAndUpcomingEventsViewModel
            {
                RecentEvents =
                    (await eventRepository.GetUpcomingAndRecentEventsAsync())
                    .ToList()
            };

            return View(model);
        }
    }
}