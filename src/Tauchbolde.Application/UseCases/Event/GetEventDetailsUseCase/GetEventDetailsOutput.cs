using JetBrains.Annotations;
using Tauchbolde.Domain.Types;

namespace Tauchbolde.Application.UseCases.Event.GetEventDetailsUseCase
{
    public class GetEventDetailsOutput
    {
        public GetEventDetailsOutput(Guid eventId,
            [NotNull] string name,
            Guid organisatorId,
            [NotNull] string organisatorFullName,
            string location,
            string meetingPoint,
            string description,
            DateTime startTime,
            DateTime? endTime,
            bool canceled,
            bool deleted,
            [NotNull] IEnumerable<ParticipantOutput> participants,
            [NotNull] IEnumerable<CommentOutput> comments,
            bool allowEdit,
            ParticipantStatus currentUserStatus,
            string currentUserNote,
            string currentUserBuddyTeamName,
            int currentUserCountPeople)
        {
            EventId = eventId;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            OrganisatorId = organisatorId;
            OrganisatorFullName = organisatorFullName ?? throw new ArgumentNullException(nameof(organisatorFullName));
            Location = location;
            MeetingPoint = meetingPoint;
            Description = description;
            StartTime = startTime;
            EndTime = endTime;
            Canceled = canceled;
            Deleted = deleted;
            Participants = participants ?? throw new ArgumentNullException(nameof(participants));
            Comments = comments ?? throw new ArgumentNullException(nameof(comments));
            AllowEdit = allowEdit;
            CurrentUserStatus = currentUserStatus;
            CurrentUserNote = currentUserNote;
            CurrentUserBuddyTeamName = currentUserBuddyTeamName;
            CurrentUserCountPeople = currentUserCountPeople;
        }

        public Guid EventId { get; }
        [NotNull] public string Name { get; }
        public Guid OrganisatorId { get; }
        [NotNull] public string OrganisatorFullName { get; }
        public string Location { get; }
        public string MeetingPoint { get; }
        public string Description { get; }
        public DateTime StartTime { get; }
        public DateTime? EndTime { get; }
        public bool Canceled { get; }
        public bool Deleted { get; }
        public bool AllowEdit { get; }
        public ParticipantStatus CurrentUserStatus { get; }
        public string CurrentUserNote { get; }
        public string CurrentUserBuddyTeamName { get; }

        [NotNull] public IEnumerable<ParticipantOutput> Participants { get; }
        [NotNull] public IEnumerable<CommentOutput> Comments { get; }
        public int CurrentUserCountPeople { get; }
    }
}