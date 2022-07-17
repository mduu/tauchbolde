using JetBrains.Annotations;

namespace Tauchbolde.InterfaceAdapters.MVC.Presenters.Events.Details
{
    public class EventViewModel
    {
        public EventViewModel(bool allowEdit,
            Guid eventId,
            [NotNull] string name,
            Guid organisatorId,
            [NotNull] string organisatorFullName,
            [CanBeNull] string location,
            [CanBeNull] string meetingPoint,
            [CanBeNull] string description,
            [NotNull] string startEndTime,
            [NotNull] IEnumerable<EventCommentViewModel> comments,
            [NotNull] EventParticipationViewModel participations)
        {
            AllowEdit = allowEdit;
            EventId = eventId;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            OrganisatorId = organisatorId;
            OrganisatorFullName = organisatorFullName ?? throw new ArgumentNullException(nameof(organisatorFullName));
            Location = location ?? "";
            MeetingPoint = meetingPoint ?? "";
            Description = description ?? "";
            StartEndTime = startEndTime ?? throw new ArgumentNullException(nameof(startEndTime));
            Comments = comments ?? throw new ArgumentNullException(nameof(comments));
            Participations = participations ?? throw new ArgumentNullException(nameof(participations));
        }

        public bool AllowEdit { get; set; }

        public Guid EventId { get; }
        [NotNull] public string Name { get; }
        public Guid OrganisatorId { get; }
        [NotNull] public string OrganisatorFullName { get; }
        [NotNull] public string Location { get; }
        [NotNull] public string MeetingPoint { get; }
        [NotNull] public string Description { get; }
        [NotNull] public string StartEndTime { get; }

        [NotNull] public IEnumerable<EventCommentViewModel> Comments { get; }
        [NotNull] public EventParticipationViewModel Participations { get; }
    }
}
