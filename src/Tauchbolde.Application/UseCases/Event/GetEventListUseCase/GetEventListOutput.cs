using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Tauchbolde.Application.UseCases.Event.GetEventListUseCase
{
    public class GetEventListOutput
    {
        public GetEventListOutput([NotNull] IEnumerable<EventRow> rows)
        {
            Rows = rows ?? throw new ArgumentNullException(nameof(rows));
        }

        public IEnumerable<EventRow> Rows { get; }
        
        public class EventRow
        {
            public EventRow(
                Guid eventId,
                DateTime startTime,
                DateTime? endTime,
                [NotNull] string title,
                [NotNull] string location,
                [NotNull] string meetingPoint)
            {
                EventId = eventId;
                StartTime = startTime;
                EndTime = endTime;
                Title = title ?? throw new ArgumentNullException(nameof(title));
                Location = location ?? throw new ArgumentNullException(nameof(location));
                MeetingPoint = meetingPoint ?? throw new ArgumentNullException(nameof(meetingPoint));
            }

            public Guid EventId { get; }
            public DateTime StartTime { get; }
            public DateTime? EndTime { get; }
            [NotNull] public string Title { get; }
            [NotNull] public string Location { get; }
            [NotNull] public string MeetingPoint { get; }
        }
    }
}