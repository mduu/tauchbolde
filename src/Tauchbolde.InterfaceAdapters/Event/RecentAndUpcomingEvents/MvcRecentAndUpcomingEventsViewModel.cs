using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Tauchbolde.InterfaceAdapters.Event.RecentAndUpcomingEvents
{
    public class MvcRecentAndUpcomingEventsViewModel
    {
        public MvcRecentAndUpcomingEventsViewModel([NotNull] IEnumerable<Row> rows)
        {
            Rows = rows ?? throw new ArgumentNullException(nameof(rows));
        }

        public IEnumerable<Row> Rows { get; }
        
        public class Row
        {
            public Row(Guid eventId, [NotNull] string startEndTime, [NotNull] string title)
            {
                EventId = eventId;
                StartEndTime = startEndTime ?? throw new ArgumentNullException(nameof(startEndTime));
                Title = title ?? throw new ArgumentNullException(nameof(title));
            }

            public Guid EventId { get; }
            [NotNull] public string StartEndTime { get; }
            [NotNull] public string Title { get; }
        }
    }
}