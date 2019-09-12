using System;
using JetBrains.Annotations;
using MediatR;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Application.UseCases.Event.NewEventUseCase
{
    public class NewEvent : IRequest<UseCaseResult<Guid>>
    {
        public NewEvent(
            [NotNull] string currentUserName,
            DateTime startTime,
            DateTime? endTime,
            [NotNull] string title,
            [NotNull] string location,
            [NotNull] string meetingPoint,
            [NotNull] string description)
        {
            CurrentUserName = currentUserName ?? throw new ArgumentNullException(nameof(currentUserName));
            StartTime = startTime;
            EndTime = endTime;
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Location = location ?? throw new ArgumentNullException(nameof(location));
            MeetingPoint = meetingPoint ?? throw new ArgumentNullException(nameof(meetingPoint));
            Description = description ?? throw new ArgumentNullException(nameof(description));
        }

        [NotNull] public string CurrentUserName { get; }
        public DateTime StartTime { get; }
        public DateTime? EndTime { get; }
        [NotNull] public string Title { get; }
        [NotNull] public string Location { get; }
        [NotNull] public string MeetingPoint { get; }
        [NotNull] public string Description { get; }
    }
}