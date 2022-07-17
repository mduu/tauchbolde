using JetBrains.Annotations;
using MediatR;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Application.UseCases.Event.GetRecentAndUpcomingEventsUseCase
{
    [UsedImplicitly]
    internal class GetRecentAndUpcomingEventsInteractor : IRequestHandler<GetRecentAndUpcomingEvents, UseCaseResult>
    {
        [NotNull] private readonly IEventRepository repository;

        public GetRecentAndUpcomingEventsInteractor([NotNull] IEventRepository repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<UseCaseResult> Handle([NotNull] GetRecentAndUpcomingEvents request, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var events = await repository.GetUpcomingAndRecentEventsAsync();

            request.OutputPort.Output(
                new RecentAndUpcomingEventsOutput(
                    events.Select(e => new RecentAndUpcomingEventsOutput.Row(
                        e.Id,
                        e.StartTime,
                        e.EndTime,
                        e.Name))));

            return UseCaseResult.Success();
        }
    }
}