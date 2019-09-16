using System;
using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;

namespace Tauchbolde.InterfaceAdapters.Event.EditDetails
{
    public class MvcEventEditDetailsViewModel
    {
        public MvcEventEditDetailsViewModel(
            Guid eventId,
            [NotNull] string organizatorName,
            [NotNull] string organizatorEmail,
            [CanBeNull] string organizatorAvatarId,
            [NotNull] string startTime,
            [CanBeNull] string endTime,
            [NotNull] string title,
            [NotNull] string location,
            [NotNull] string meetingPoint,
            [NotNull] string description)
        {
            EventId = eventId;
            OrganizatorName = organizatorName ?? throw new ArgumentNullException(nameof(organizatorName));
            OrganizatorEmail = organizatorEmail ?? throw new ArgumentNullException(nameof(organizatorEmail));
            OrganizatorAvatarId = organizatorAvatarId;
            StartTime = startTime ?? throw new ArgumentNullException(nameof(startTime));
            EndTime = endTime;
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Location = location ?? throw new ArgumentNullException(nameof(location));
            MeetingPoint = meetingPoint ?? throw new ArgumentNullException(nameof(meetingPoint));
            Description = description ?? throw new ArgumentNullException(nameof(description));
        }

        public Guid EventId { get; }

        [NotNull]
        [Display(Name = "Organisator")]
        public string OrganizatorName { get; }

        [NotNull] public string OrganizatorEmail { get; }
        [CanBeNull] public string OrganizatorAvatarId { get; }

        [NotNull]
        [Display(Name = "Startet um")]
        public string StartTime { get; }

        [CanBeNull]
        [Display(Name = "Endet um")]
        public string EndTime { get; }

        [NotNull]
        [Display(Name = "Aktivitätstitel")]
        public string Title { get; }

        [NotNull] [Display(Name = "Ort / TP")] public string Location { get; }

        [NotNull]
        [Display(Name = "Treffpunkt")]
        public string MeetingPoint { get; }

        [NotNull]
        [Display(Name = "Beschreibung")]
        public string Description { get; }
    }
}