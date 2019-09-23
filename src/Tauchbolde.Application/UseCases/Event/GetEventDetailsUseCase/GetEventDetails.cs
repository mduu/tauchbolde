using System;
using JetBrains.Annotations;
using MediatR;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Application.UseCases.Event.GetEventDetailsUseCase
{
    public class GetEventDetails : IRequest<UseCaseResult>
    {
        public GetEventDetails(Guid eventId, [NotNull] IEventDetailsOutputPort outputPort)
        {
            EventId = eventId;
            OutputPort = outputPort ?? throw new ArgumentNullException(nameof(outputPort));
        }

        public Guid EventId { get; }
        public IEventDetailsOutputPort OutputPort { get; }
    }
}