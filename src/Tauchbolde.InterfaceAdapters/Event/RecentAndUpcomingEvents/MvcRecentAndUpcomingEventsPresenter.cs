using System;
using System.Linq;
using JetBrains.Annotations;
using Tauchbolde.Application.UseCases.Event.GetRecentAndUpcomingEventsUseCase;
using Tauchbolde.SharedKernel.Extensions;

namespace Tauchbolde.InterfaceAdapters.Event.RecentAndUpcomingEvents
{
    public class MvcRecentAndUpcomingEventsPresenter : IRecentAndUpcomingEventsOutputPort
    {
        private MvcRecentAndUpcomingEventsViewModel viewModel;
        public void Output([NotNull] RecentAndUpcomingEventsOutput interactorOutput)
        {
            if (interactorOutput == null) throw new ArgumentNullException(nameof(interactorOutput));
            
            viewModel = new MvcRecentAndUpcomingEventsViewModel(
                interactorOutput.Rows.Select(r =>
                    new MvcRecentAndUpcomingEventsViewModel.Row(
                        r.EventId,
                        r.StartTime.FormatTimeRange(r.EndTime),
                        r.Title)));
        }

        public MvcRecentAndUpcomingEventsViewModel GetViewModel() => viewModel;
    }
}