using System;
using JetBrains.Annotations;
using MediatR;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Application.UseCases.Event.GetEventEditDetailsUseCase
{
    public class GetEventEditDetails : IRequest<UseCaseResult>
    {
        public GetEventEditDetails(
            [NotNull] string currentUserName,
            Guid eventId,
            [NotNull] IEventEditDetailsOutputPort outputPort)
        {
            CurrentUserName = currentUserName ?? throw new ArgumentNullException(nameof(currentUserName));
            EventId = eventId;
            OutputPort = outputPort ?? throw new ArgumentNullException(nameof(outputPort));
        }

        [NotNull] public string CurrentUserName { get; }
        public Guid EventId { get; }
        [NotNull] public IEventEditDetailsOutputPort OutputPort { get; }
    }
}