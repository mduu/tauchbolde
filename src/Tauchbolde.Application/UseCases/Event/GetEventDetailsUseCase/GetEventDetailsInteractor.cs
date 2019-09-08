using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using Microsoft.Extensions.Logging;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.Domain.Types;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Application.UseCases.Event.GetEventDetailsUseCase
{
    [UsedImplicitly]
    public class GetEventDetailsInteractor : IRequestHandler<GetEventDetails, UseCaseResult>
    {
        [NotNull] private readonly ILogger logger;
        [NotNull] private readonly IEventRepository eventRepository;
        [NotNull] private readonly IDiverRepository diverRepository;
        [NotNull] private readonly IParticipantRepository participantRepository;

        public GetEventDetailsInteractor(
            [NotNull] ILogger<GetEventDetailsInteractor> logger,
            [NotNull] IEventRepository eventRepository, 
            [NotNull] IDiverRepository diverRepository,
            [NotNull] IParticipantRepository participantRepository)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.eventRepository = eventRepository ?? throw new ArgumentNullException(nameof(eventRepository));
            this.diverRepository = diverRepository ?? throw new ArgumentNullException(nameof(diverRepository));
            this.participantRepository = participantRepository ?? throw new ArgumentNullException(nameof(participantRepository));
        }
        
        public async Task<UseCaseResult> Handle([NotNull] GetEventDetails request, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var currentDiver = await diverRepository.FindByUserNameAsync(request.CurrentUserName);
            if (currentDiver == null)
            {
                logger.LogError("No Diver instance found for current user [{username}]!", request.CurrentUserName);
                return UseCaseResult.Fail();
            }
            
            var evt = await eventRepository.FindByIdAsync(request.EventId);
            if (evt == null)
            {
                logger.LogError("Event not found for [{id}]!", request.EventId);
                return UseCaseResult.NotFound();
            }

            var currentUserParticipation = await participantRepository.GetParticipationForEventAndUserAsync(currentDiver, request.EventId);

            request.OutputPort.Output(new GetEventDetailsOutput(
                evt.Id,
                evt.Name,
                evt.OrganisatorId,
                evt.Organisator.Fullname,
                evt.Location,
                evt.MeetingPoint,
                evt.Description,
                evt.StartTime,
                evt.EndTime,
                evt.Canceled,
                evt.Deleted,
                evt.Participants.Select(p => new ParticipantOutput(
                    p.ParticipatingDiver.Fullname,
                    p.ParticipatingDiver.User.Email,
                    p.ParticipatingDiver.AvatarId,
                    p.BuddyTeamName,
                    p.Status,
                    p.CountPeople,
                    p.Note)),
                evt.Comments.Select(c => new CommentOutput(
                    c.Id,
                    c.AuthorId,
                    c.Author.Fullname,
                    c.Author.User.Email,
                    c.Author.AvatarId,
                    c.CreateDate,
                    c.ModifiedDate,
                    c.Text,
                    c.AuthorId == currentDiver.Id,
                    c.AuthorId == currentDiver.Id)),
                evt.OrganisatorId == currentDiver.Id,
                currentUserParticipation?.Status ?? ParticipantStatus.None,
                currentUserParticipation.Note,
                currentUserParticipation.BuddyTeamName,
                currentUserParticipation.CountPeople));
            
            return UseCaseResult.Success();
        }
    }
}