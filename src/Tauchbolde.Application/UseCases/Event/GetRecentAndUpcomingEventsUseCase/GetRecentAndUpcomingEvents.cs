using JetBrains.Annotations;
using MediatR;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Application.UseCases.Event.GetRecentAndUpcomingEventsUseCase
{
    public class GetRecentAndUpcomingEvents : IRequest<UseCaseResult>
    {
        public GetRecentAndUpcomingEvents([NotNull] IRecentAndUpcomingEventsOutputPort outputPort)
        {
            OutputPort = outputPort ?? throw new ArgumentNullException(nameof(outputPort));
        }

        [NotNull] public IRecentAndUpcomingEventsOutputPort OutputPort { get; }
    }
}