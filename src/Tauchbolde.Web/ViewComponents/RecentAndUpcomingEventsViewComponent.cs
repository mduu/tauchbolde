using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Tauchbolde.Application.UseCases.Event.GetRecentAndUpcomingEventsUseCase;
using Tauchbolde.InterfaceAdapters.Event.RecentAndUpcomingEvents;

namespace Tauchbolde.Web.ViewComponents
{
    public class RecentAndUpcomingEventsViewComponent : ViewComponent
    {
        [NotNull] private readonly IMediator mediator;
        [NotNull] private readonly ILogger logger;

        public RecentAndUpcomingEventsViewComponent(
            [NotNull] IMediator mediator,
            [NotNull] ILogger<RecentAndUpcomingEventsViewComponent> logger)
        {
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