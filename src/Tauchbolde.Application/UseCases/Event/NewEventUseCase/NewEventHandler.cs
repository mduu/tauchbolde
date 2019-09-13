using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Application.UseCases.Event.NewEventUseCase
{
    [UsedImplicitly]
    internal class NewEventHandler : IRequestHandler<NewEvent, UseCaseResult<Guid>>
    {
        [NotNull] private readonly IDiverRepository diverRepository;
        [NotNull] private readonly IEventRepository eventRepository;
        
        public NewEventHandler(
            [NotNull] IDiverRepository diverRepository,
            [NotNull] IEventRepository eventRepository)
        {
            this.diverRepository = diverRepository ?? throw new ArgumentNullException(nameof(diverRepository));
            this.eventRepository = eventRepository ?? throw new ArgumentNullException(nameof(eventRepository));
        }

        public async Task<UseCaseResult<Guid>> Handle(NewEvent request, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var diver = await diverRepository.FindByUserNameAsync(request.CurrentUserName);
            if (diver == null)
            {
                return UseCaseResult<Guid>.NotFound();
            }
            
            var evt = new Domain.Entities.Event(
                request.Title,
                request.Description,
                request.Location,
                request.MeetingPoint,
                request.StartTime,
                request.EndTime,
                diver.Id);

            await eventRepository.InsertAsync(evt);

            return UseCaseResult<Guid>.Success(evt.Id);
        }
    }
}