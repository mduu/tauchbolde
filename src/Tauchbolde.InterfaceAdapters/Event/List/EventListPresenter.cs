using System.Linq;
using Tauchbolde.Application.UseCases.Event.GetEventListUseCase;
using Tauchbolde.SharedKernel.Extensions;

namespace Tauchbolde.InterfaceAdapters.Event.List
{
    public class EventListPresenter : IEventListOutputPort
    {
        private EventListViewModel viewModel;
        
        public void Output(GetEventListOutput interactorOutput)
        {
            viewModel = new EventListViewModel(
                interactorOutput.Rows.Select(r => new EventListViewModel.RowViewModel(
                    r.EventId,
                    r.StartTime.FormatTimeRange(r.EndTime),
                    r.Title,
                    r.Location,
                    r.MeetingPoint
                )));
        }

        public EventListViewModel GetViewModel() => viewModel;
    }
}