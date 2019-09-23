using System;
using JetBrains.Annotations;
using MediatR;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Application.UseCases.Event.GetEventEditDetailsUseCase
{
    public class GetEventEditDetails : IRequest<UseCaseResult>
    {
        public GetEventEditDetails(
            Guid? eventId,
            [NotNull] IEventEditDetailsOutputPort outputPort)
        {
            EventId = eventId;
            OutputPort = outputPort ?? throw new ArgumentNullException(nameof(outputPort));
        }

        public Guid? EventId { get; }
        [NotNull] public IEventEditDetailsOutputPort OutputPort { get; }
    }
}