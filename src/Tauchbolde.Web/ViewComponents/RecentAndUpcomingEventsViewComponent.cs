using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tauchbolde.Common.Repositories;
using Tauchbolde.Web.Models.ViewComponentModels;

namespace Tauchbolde.Web.ViewComponents
{
    public class RecentAndUpcomingEventsViewComponent : ViewComponent
    {
        private readonly IEventRepository _eventRepository;

        public RecentAndUpcomingEventsViewComponent(
            IEventRepository eventRepository)
        {
            if (eventRepository == null) throw new ArgumentNullException(nameof(eventRepository));

            _eventRepository = eventRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = new RecentAndUpcomingEventsViewModel
            {
                RecentEvents =
                    (await _eventRepository.GetUpcomingAndRecentEventsAsync())
                    .ToList()
            };

            return View(model);
        }
    }
}