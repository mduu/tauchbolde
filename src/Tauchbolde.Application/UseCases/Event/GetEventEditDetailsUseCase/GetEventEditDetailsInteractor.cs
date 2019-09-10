using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation.Results;
using JetBrains.Annotations;
using MediatR;
using Microsoft.Extensions.Logging;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Application.UseCases.Event.GetEventEditDetailsUseCase
{
    [UsedImplicitly]
    internal class GetEventEditDetailsInteractor : IRequestHandler<GetEventEditDetails, UseCaseResult>
    {
        [NotNull] private readonly ILogger logger;
        [NotNull] private readonly IEventRepository eventRepository;
        [NotNull] private readonly IDiverRepository diverRepository;

        public GetEventEditDetailsInteractor(
            [NotNull] ILogger<GetEventEditDetailsInteractor> logger,
            [NotNull] IEventRepository eventRepository,
            [NotNull] IDiverRepository diverRepository)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.eventRepository = eventRepository ?? throw new ArgumentNullException(nameof(eventRepository));
            this.diverRepository = diverRepository ?? throw new ArgumentNullException(nameof(diverRepository));
        }

        public async Task<UseCaseResult> Handle([NotNull] GetEventEditDetails request, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var diver = await diverRepository.FindByUserNameAsync(request.CurrentUserName);
            if (diver == null)
            {
                logger.LogError("Diver with Name [{username}] not found!", request.CurrentUserName);
                return UseCaseResult.NotFound();
            }

            var evt = await eventRepository.FindByIdAsync(request.EventId);
            if (evt == null)
            {
                logger.LogError("Event with ID [{id}] not found!", request.EventId);
                return UseCaseResult.NotFound();
            }

            if (diver.Id != evt.OrganisatorId)
            {
                logger.LogError("Diver [{username}] is not allowed to edit event with ID [{id}]!", request.CurrentUserName, request.EventId);
                return UseCaseResult.Fail(
                    new List<ValidationFailure>
                    {
                        new ValidationFailure("eventId", "Nur der Organisator ist berechtigt diese Aktivität zu bearbeiten!")
                    },
                    ResultCategory.AccessDenied);
            }
            
            request.OutputPort.Output(
                new EventEditDetailsOutput(
                    evt.Id,
                    evt.Name,
                    evt.Location,
                    evt.MeetingPoint,
                    evt.Description,
                    evt.StartTime,
                    evt.EndTime,
                    evt.Organisator.Fullname,
                    evt.Organisator.User.Email,
                    evt.Organisator.AvatarId));

            return UseCaseResult.Success();
        }
    }
}