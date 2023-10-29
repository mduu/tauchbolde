using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;

namespace Tauchbolde.InterfaceAdapters.MVC.Presenters.Events.EditDetails
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

        [UsedImplicitly]
        public MvcEventEditDetailsViewModel()
        {
        }

        public Guid EventId { get; set; }

        [Display(Name = "Organisator")]
        public string OrganizatorName { get; set; }

        public string OrganizatorEmail { get; set; }
        public string OrganizatorAvatarId { get; set; }

        [Display(Name = "Startet um")]
        public string StartTime { get; set; }

        [Display(Name = "Endet um")]
        public string EndTime { get; set; }

        [Display(Name = "Aktivitätstitel")]
        public string Title { get; set; }

        [Display(Name = "Ort / TP")] public string Location { get; set; }

        [Display(Name = "Treffpunkt")]
        public string MeetingPoint { get; set; }

        [Display(Name = "Beschreibung")]
        public string Description { get; set; }
    }
}