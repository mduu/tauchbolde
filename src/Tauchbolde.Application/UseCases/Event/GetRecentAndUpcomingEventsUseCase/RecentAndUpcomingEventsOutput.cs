using JetBrains.Annotations;

namespace Tauchbolde.Application.UseCases.Event.GetRecentAndUpcomingEventsUseCase
{
    public class RecentAndUpcomingEventsOutput
    {
        public RecentAndUpcomingEventsOutput([NotNull] IEnumerable<Row> rows)
        {
            Rows = rows ?? throw new ArgumentNullException(nameof(rows));
        }

        public IEnumerable<Row> Rows { get; }

        public class Row
        {
            public Row(Guid eventId, DateTime startTime, DateTime? endTime, [NotNull] string title)
            {
                EventId = eventId;
                StartTime = startTime;
                EndTime = endTime;
                Title = title ?? throw new ArgumentNullException(nameof(title));
            }

            public Guid EventId { get; }
            public DateTime StartTime { get; }
            public DateTime? EndTime { get; }
            [NotNull] public string Title { get; }
        }
    }
}