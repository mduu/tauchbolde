using JetBrains.Annotations;
using MediatR;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Application.UseCases.Event.GetEventListUseCase
{
    [UsedImplicitly]
    internal class GetEventListInteractor : IRequestHandler<GetEventList, UseCaseResult>
    {
        private readonly IEventRepository repository;
        
        public GetEventListInteractor([NotNull] IEventRepository repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }
        
        public async Task<UseCaseResult> Handle([NotNull] GetEventList request, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var eventList = await repository.GetUpcomingEventsAsync();
            
            request.OutputPort.Output(new GetEventListOutput(
                eventList.Select(e => new GetEventListOutput.EventRow(
                    e.Id,
                    e.StartTime,
                    e.EndTime,
                    e.Name ?? "",
                    e.Location ?? "",
                    e.MeetingPoint ?? ""
                ))));
            
            return UseCaseResult.Success();
        }
    }
}