using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.Domain.Entities;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Application.UseCases.Event.ChangeParticipationUseCase
{
    [UsedImplicitly]
    internal class ChangeParticipationInteractor : IRequestHandler<ChangeParticipation, UseCaseResult>
    {
        private readonly IDiverRepository diverRepository;
        private readonly IParticipantRepository participantRepository;

        public ChangeParticipationInteractor(
            [NotNull] IDiverRepository diverRepository,
            [NotNull] IParticipantRepository participantRepository)
        {
            this.diverRepository = diverRepository ?? throw new ArgumentNullException(nameof(diverRepository));
            this.participantRepository = participantRepository ?? throw new ArgumentNullException(nameof(participantRepository));
        }
        
        public async Task<UseCaseResult> Handle([NotNull] ChangeParticipation request, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var diver = await diverRepository.FindByUserNameAsync(request.Username);
            var participant = await participantRepository.GetParticipationForEventAndUserAsync(diver, request.EventId);
            if (participant == null)
            {
                participant = new Participant(
                    request.EventId,
                    diver.Id,
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