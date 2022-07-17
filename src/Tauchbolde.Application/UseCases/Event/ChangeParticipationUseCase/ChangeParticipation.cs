using MediatR;
using Tauchbolde.Domain.Types;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Application.UseCases.Event.ChangeParticipationUseCase
{
    public class ChangeParticipation : IRequest<UseCaseResult>
    {
        public ChangeParticipation(Guid eventId,
            ParticipantStatus status,
            int numberOfPeople,
            string note,
            string buddyTeamName)
        {
            EventId = eventId;
            Status = status;
            NumberOfPeople = numberOfPeople;
            Note = note;
            BuddyTeamName = buddyTeamName;
        }

        public Guid EventId { get; }
        public ParticipantStatus Status { get; }
        public int NumberOfPeople { get; }
        public string Note { get; }
        public string BuddyTeamName { get; }
    }
}