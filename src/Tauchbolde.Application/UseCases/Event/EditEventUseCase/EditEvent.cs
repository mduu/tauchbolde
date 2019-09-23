using System;
using JetBrains.Annotations;
using MediatR;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Application.UseCases.Event.EditEventUseCase
{
    public class EditEvent : IRequest<UseCaseResult<Guid>>
    {
        public EditEvent(
            Guid eventId,
            DateTime startTime,
            DateTime? endTime,
            [NotNull] string title,
            [NotNull] string location,
            [NotNull] string meetingPoint,
            [NotNull] string description)
        {
            EventId = eventId;
            StartTime = startTime;
            EndTime = endTime;
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Location = location ?? throw new ArgumentNullException(nameof(location));
            MeetingPoint = meetingPoint ?? throw new ArgumentNullException(nameof(meetingPoint));
            Description = description ?? throw new ArgumentNullException(nameof(description));
        }

        public Guid EventId { get; }
        public DateTime StartTime { get; }
        public DateTime? EndTime { get; }
        [NotNull] public string Title { get; }
        [NotNull] public string Location { get; }
        [NotNull] public string MeetingPoint { get; }
        [NotNull] public string Description { get; }
    }
}