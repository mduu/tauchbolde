using JetBrains.Annotations;
using MediatR;
using Microsoft.Extensions.Logging;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.Application.Services.Core;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Application.UseCases.Event.EditEventUseCase
{
    [UsedImplicitly]
    internal class EditEventInteractor : IRequestHandler<EditEvent, UseCaseResult<Guid>>
    {
        [NotNull] private readonly ILogger logger;
        [NotNull] private readonly IEventRepository eventRepository;
        [NotNull] private readonly ICurrentUser currentUser;

        public EditEventInteractor(
            [NotNull] ILogger<EditEventInteractor> logger,
            [NotNull] IEventRepository eventRepository,
            [NotNull] ICurrentUser currentUser)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.eventRepository = eventRepository ?? throw new ArgumentNullException(nameof(eventRepository));
            this.currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        }

        public async Task<UseCaseResult<Guid>> Handle([NotNull] EditEvent request, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var currentDiver = await currentUser.GetCurrentDiverAsync();
            if (currentDiver == null)
            {
                logger.LogError("No diver record found for username [{username}]!", currentUser.Username);
                return UseCaseResult<Guid>.NotFound();
            }

            var evt = await eventRepository.FindByIdAsync(request.EventId);
            if (evt == null)
            {
                var newEvent = new Domain.Entities.Event(
                    request.Title,
                    request.Description,
                    request.Location,
                    request.MeetingPoint,
                    request.StartTime,
                    request.EndTime,
                    currentDiver.Id);
                
                await eventRepository.InsertAsync(newEvent);
                return UseCaseResult<Guid>.Success(newEvent.Id);
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
                return UseCaseResult<Guid>.Fail(resultCategory: ResultCategory.AccessDenied);
            }

            await eventRepository.UpdateAsync(evt);
            return UseCaseResult<Guid>.Success(evt.Id);
        }
    }
}