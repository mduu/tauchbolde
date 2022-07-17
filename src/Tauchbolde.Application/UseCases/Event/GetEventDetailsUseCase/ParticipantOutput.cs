using JetBrains.Annotations;
using Tauchbolde.Domain.Types;

namespace Tauchbolde.Application.UseCases.Event.GetEventDetailsUseCase
{
    public class ParticipantOutput
    {
        public ParticipantOutput(
            [NotNull] string name,
            [NotNull] string email,
            string avatarId,
            [CanBeNull] string buddyTeamName,
            ParticipantStatus status,
            int countPeople,
            [CanBeNull] string note)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Email = email ?? throw new ArgumentNullException(nameof(email));
            BuddyTeamName = buddyTeamName ?? "";
            Status = status;
            CountPeople = countPeople;
            AvatarId = avatarId;
            Note = note ?? "";
        }

        [NotNull] public string Name { get; }
        public string Email { get; }
        [NotNull] public string BuddyTeamName { get; }
        public ParticipantStatus Status { get; } // TODO Change to non-domain enum
        public int CountPeople { get; }
        public string AvatarId { get; }
        [NotNull] public string Note { get; }
    }
}