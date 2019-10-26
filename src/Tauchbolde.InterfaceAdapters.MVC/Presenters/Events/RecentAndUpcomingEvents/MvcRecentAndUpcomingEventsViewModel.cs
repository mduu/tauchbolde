using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Tauchbolde.InterfaceAdapters.MVC.Presenters.Events.RecentAndUpcomingEvents
{
    public class MvcRecentAndUpcomingEventsViewModel
    {
        public MvcRecentAndUpcomingEventsViewModel(
            [NotNull] IEnumerable<Row> recentEvents,
            [NotNull] IEnumerable<Row> upcomingEvents)
        {
            RecentEvents = recentEvents ?? throw new ArgumentNullException(nameof(recentEvents));
            UpcomingEvents = upcomingEvents ?? throw new ArgumentNullException(nameof(upcomingEvents));
        }

        public IEnumerable<Row> RecentEvents { get; }
        public IEnumerable<Row> UpcomingEvents { get; }
        
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