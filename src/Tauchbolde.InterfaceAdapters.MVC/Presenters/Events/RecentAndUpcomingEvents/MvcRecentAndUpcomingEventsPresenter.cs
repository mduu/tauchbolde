using JetBrains.Annotations;
using Tauchbolde.Application.UseCases.Event.GetRecentAndUpcomingEventsUseCase;
using Tauchbolde.SharedKernel.Extensions;

namespace Tauchbolde.InterfaceAdapters.MVC.Presenters.Events.RecentAndUpcomingEvents
{
    public class MvcRecentAndUpcomingEventsPresenter : IRecentAndUpcomingEventsOutputPort
    {
        private MvcRecentAndUpcomingEventsViewModel viewModel;

        public void Output([NotNull] RecentAndUpcomingEventsOutput interactorOutput)
        {
            if (interactorOutput == null) throw new ArgumentNullException(nameof(interactorOutput));

            var upcomingEvents = interactorOutput.Rows.Where(e => e.StartTime > DateTime.Now).ToList();
            var recentEvents = interactorOutput.Rows.OrderByDescending(e => e.StartTime).Where(e => e.StartTime < DateTime.Now).ToList();

            viewModel = new MvcRecentAndUpcomingEventsViewModel(
                recentEvents.Select(r =>
                    new MvcRecentAndUpcomingEventsViewModel.Row(
                        r.EventId,
                        r.StartTime.FormatTimeRange(r.EndTime),
                        r.Title)),
                upcomingEvents.Select(r =>
                    new MvcRecentAndUpcomingEventsViewModel.Row(
                        r.EventId,
                        r.StartTime.FormatTimeRange(r.EndTime),
                        r.Title)));
        }
    
        public MvcRecentAndUpcomingEventsViewModel GetViewModel() => viewModel;
    }
}