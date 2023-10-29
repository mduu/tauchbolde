using JetBrains.Annotations;
using MediatR;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.Application.Services.Core;
using Tauchbolde.Domain.Entities;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Application.UseCases.Event.ChangeParticipationUseCase
{
    [UsedImplicitly]
    internal class ChangeParticipationInteractor : IRequestHandler<ChangeParticipation, UseCaseResult>
    {
        private readonly IParticipantRepository participantRepository;
        private readonly ICurrentUser currentUser;

        public ChangeParticipationInteractor(
            [NotNull] IParticipantRepository participantRepository,
            [NotNull] ICurrentUser currentUser)
        {
            this.participantRepository = participantRepository ?? throw new ArgumentNullException(nameof(participantRepository));
            this.currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        }
        
        public async Task<UseCaseResult> Handle([NotNull] ChangeParticipation request, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var currentDiver = await currentUser.GetCurrentDiverAsync();
            var participant = await participantRepository.GetParticipationForEventAndUserAsync(currentDiver, request.EventId);
            if (participant == null)
            {
                participant = new Participant(
                    request.EventId,
                    currentDiver.Id,
                    request.Status,
                    request.BuddyTeamName,
                    request.NumberOfPeople,
                    request.Note);
                
                await participantRepository.InsertAsync(participant);
            }
            else
            {
                participant.Edit(
                    request.Status,
                    request.BuddyTeamName,
                    request.NumberOfPeople,
                    request.Note);
                
                await participantRepository.UpdateAsync(participant);
            }
            
            return UseCaseResult.Success();
        }
    }
}