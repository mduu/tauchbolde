using Tauchbolde.Application.UseCases.Event.GetEventListUseCase;
using Tauchbolde.SharedKernel.Extensions;

namespace Tauchbolde.InterfaceAdapters.MVC.Presenters.Events.List
{
    public class MvcEventListPresenter : IEventListOutputPort
    {
        private MvcEventListViewModel viewModel;
        
        public void Output(GetEventListOutput interactorOutput)
        {
            viewModel = new MvcEventListViewModel(
                interactorOutput.Rows.Select(r => new MvcEventListViewModel.RowViewModel(
                    r.EventId,
                    r.StartTime.FormatTimeRange(r.EndTime),
                    r.Title,
                    r.Location,
                    r.MeetingPoint
                )));
        }

        public MvcEventListViewModel GetViewModel() => viewModel;
    }
}