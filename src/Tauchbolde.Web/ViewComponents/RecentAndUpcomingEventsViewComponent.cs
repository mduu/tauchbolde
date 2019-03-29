using System;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Tauchbolde.Common.DomainServices.Events;
using Tauchbolde.Web.Models.ViewComponentModels;

namespace Tauchbolde.Web.ViewComponents
{
    public class RecentAndUpcomingEventsViewComponent : ViewComponent
    {
        [NotNull] private readonly IEventService eventService;

        public RecentAndUpcomingEventsViewComponent([NotNull] IEventService eventService
            )
        {
            this.eventService = eventService ?? throw new ArgumentNullException(nameof(eventService));
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = new RecentAndUpcomingEventsViewModel
            {
                RecentEvents =
                    (await eventService.GetUpcomingAndRecentEventsAsync())
                    .ToList()
            };

            return View(model);
        }
    }
}