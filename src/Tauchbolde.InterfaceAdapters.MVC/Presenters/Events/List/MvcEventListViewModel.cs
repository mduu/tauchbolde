using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;

namespace Tauchbolde.InterfaceAdapters.MVC.Presenters.Events.List
{
    public class MvcEventListViewModel
    {
        public MvcEventListViewModel([NotNull] IEnumerable<RowViewModel> rows)
        {
            Rows = rows ?? throw new ArgumentNullException(nameof(rows));
        }

        [NotNull] public IEnumerable<RowViewModel> Rows { get; }

        public class RowViewModel
        {
            public RowViewModel(
                Guid eventId,
                [NotNull] string startEndTime,
                [NotNull] string title,
                [NotNull] string location,
                [NotNull] string meetingPoint)
            {
                EventId = eventId;
                StartEndTime = startEndTime ?? throw new ArgumentNullException(nameof(startEndTime));
                Title = title ?? throw new ArgumentNullException(nameof(title));
                Location = location ?? throw new ArgumentNullException(nameof(location));
                MeetingPoint = meetingPoint ?? throw new ArgumentNullException(nameof(meetingPoint));
            }

            public Guid EventId { get; }
            [NotNull] [Display(Name = "Zeit")] public string StartEndTime { get; }
            [NotNull] [Display(Name = "Titel")] public string Title { get; }
            [NotNull] [Display(Name = "Ort/TP")] public string Location { get; }
            [NotNull] [Display(Name = "Treffpunkt")] public string MeetingPoint { get; }
        }
    }
}