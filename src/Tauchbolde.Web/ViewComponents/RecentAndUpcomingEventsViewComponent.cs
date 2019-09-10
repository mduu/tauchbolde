using System;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Tauchbolde.Application.OldDomainServices.Events;
using Tauchbolde.Application.UseCases.Event.GetRecentAndUpcomingEventsUseCase;
using Tauchbolde.InterfaceAdapters.Event.RecentAndUpcomingEvents;
using Tauchbolde.Web.Models.ViewComponentModels;

namespace Tauchbolde.Web.ViewComponents
{
    public class RecentAndUpcomingEventsViewComponent : ViewComponent
    {
        [NotNull] private readonly IEventService eventService;
        [NotNull] private readonly IMediator mediator;
        [NotNull] private readonly ILogger logger;

        public RecentAndUpcomingEventsViewComponent(
            [NotNull] IEventService eventService,
            [NotNull] IMediator mediator,
            [NotNull] ILogger<RecentAndUpcomingEventsViewComponent> logger)
        {
            this.eventService = eventService ?? throw new ArgumentNullException(nameof(eventService));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var presenter = new MvcRecentAndUpcomingEventsPresenter();
            var useCaseResult = await mediator.Send(new GetRecentAndUpcomingEvents(presenter));
            if (!useCaseResult.IsSuccessful)
            {
                logger.LogError("Error while getting RecentAndUpcomingEvents!");
                return null;
            }

            return View(presenter.GetViewModel());
        }
    }
}