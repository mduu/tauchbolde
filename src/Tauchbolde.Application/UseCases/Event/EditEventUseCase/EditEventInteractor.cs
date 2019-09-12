using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using Microsoft.Extensions.Logging;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Application.UseCases.Event.EditEventUseCase
{
    [UsedImplicitly]
    public class EditEventInteractor : IRequestHandler<EditEvent, UseCaseResult>
    {
        [NotNull] private readonly ILogger logger;
        [NotNull] private readonly IEventRepository eventRepository;
        [NotNull] private readonly IDiverRepository diverRepository;
        
        public EditEventInteractor(
            [NotNull] ILogger<EditEventInteractor> logger,
            [NotNull] IEventRepository eventRepository,
            [NotNull] IDiverRepository diverRepository)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.eventRepository = eventRepository ?? throw new ArgumentNullException(nameof(eventRepository));
            this.diverRepository = diverRepository ?? throw new ArgumentNullException(nameof(diverRepository));
        }
        
        public async Task<UseCaseResult> Handle([NotNull] EditEvent request, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var currentDiver = await diverRepository.FindByUserNameAsync(request.CurrentUserName);
            if (currentDiver == null)
            {
                logger.LogError("No diver record found for username [{username}]!", request.CurrentUserName);
                return UseCaseResult.NotFound();
            }

            var evt = await eventRepository.FindByIdAsync(request.EventId);
            if (evt == null)
            {
                logger.LogError("Event with ID [{id}] not found!", request.EventId);
                return UseCaseResult.NotFound();
            }

            var wasEdited = evt.Edit(
                currentDiver.Id,
                request.Title,
                request.Description,
                request.Location,
                request.MeetingPoint,
                request.StartTime,
                request.EndTime);
            
            if (!wasEdited)
            {
                return UseCaseResult.Fail(resultCategory: ResultCategory.AccessDenied);
            }

            await eventRepository.UpdateAsync(evt);

            return UseCaseResult.Success();
        }
    }
}