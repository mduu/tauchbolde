using System;
using JetBrains.Annotations;
using MediatR;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Application.UseCases.Event.GetEventDetailsUseCase
{
    public class GetEventDetails : IRequest<UseCaseResult>
    {
        public GetEventDetails(Guid eventId, [NotNull] IEventDetailsPresenter presenter)
        {
            EventId = eventId;
            Presenter = presenter ?? throw new ArgumentNullException(nameof(presenter));
        }

        public Guid EventId { get; }
        public IEventDetailsPresenter Presenter { get; }
    }
}