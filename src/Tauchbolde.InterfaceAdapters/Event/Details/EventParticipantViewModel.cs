using Tauchbolde.Domain.Types;

namespace Tauchbolde.InterfaceAdapters.Event.Details
{
    public class EventParticipantViewModel
    {
        public EventParticipantViewModel(
            string userName,
            string userEmail,
            string userAvatarId,
            string buddyTeamName,
            string statusName,
            ParticipantStatus status,
            string note,
            int countPeople)
        {
            UserName = userName;
            UserEmail = userEmail;
            UserAvatarId = userAvatarId;
            BuddyTeamName = buddyTeamName;
            StatusName = statusName;
            Status = status;
            Note = note;
            CountPeople = countPeople;
        }

        public string UserName { get; }
        public string UserEmail { get; }
        public string UserAvatarId { get; }
        public string BuddyTeamName { get; }
        public string StatusName { get; }
        public ParticipantStatus Status { get; } // TODO Remove
        public string Note { get; }
        public int CountPeople { get; }
    }
}