using JetBrains.Annotations;

namespace Tauchbolde.Application.UseCases.Event.GetEventEditDetailsUseCase
{
    public class EventEditDetailsOutput
    {
        public EventEditDetailsOutput(
            Guid eventId,
            [NotNull] string title,
            [NotNull] string location,
            [NotNull] string meetingPoint,
            [NotNull] string description,
            DateTime startTime,
            DateTime? endTime,
            [NotNull] string organisatorName,
            [NotNull] string organisatorEmail,
            [CanBeNull] string organisatorAvatarId)
        {
            EventId = eventId;
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Location = location ?? throw new ArgumentNullException(nameof(location));
            MeetingPoint = meetingPoint ?? throw new ArgumentNullException(nameof(meetingPoint));
            Description = description ?? throw new ArgumentNullException(nameof(description));
            StartTime = startTime;
            EndTime = endTime;
            OrganisatorName = organisatorName ?? throw new ArgumentNullException(nameof(organisatorName));
            OrganisatorEmail = organisatorEmail ?? throw new ArgumentNullException(nameof(organisatorEmail));
            OrganisatorAvatarId = organisatorAvatarId;
        }

        public Guid EventId { get; }
        [NotNull] public string Title { get; }
        [NotNull] public string Location { get; }
        [NotNull] public string MeetingPoint { get; }
        [NotNull] public string Description { get; }
        public DateTime StartTime { get; }
        public DateTime? EndTime { get; }
        [NotNull] public string OrganisatorName { get; }
        [NotNull] public string OrganisatorEmail { get; }
        [CanBeNull] public string OrganisatorAvatarId { get; }
    }
}