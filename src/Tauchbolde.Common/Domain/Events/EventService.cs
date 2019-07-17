using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Tauchbolde.Common.Domain.Notifications;
using Tauchbolde.Common.Infrastructure.Telemetry;
using Tauchbolde.Entities;
using Tauchbolde.Common.Repositories;

namespace Tauchbolde.Common.Domain.Events
{
    internal class EventService : IEventService
    {
        private readonly INotificationService notificationService;
        private readonly IEventRepository eventRepository;
        private readonly ICommentRepository commentRepository;
        private readonly ITelemetryService telemetryService;

        public EventService(INotificationService notificationService,
            IEventRepository eventRepository,
            ICommentRepository commentRepository,
            ITelemetryService telemetryService)
        {
            this.notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
            this.eventRepository = eventRepository ?? throw new ArgumentNullException(nameof(eventRepository));
            this.commentRepository = commentRepository ?? throw new ArgumentNullException(nameof(commentRepository));
            this.telemetryService = telemetryService ?? throw new ArgumentNullException(nameof(telemetryService));

        }

        /// <inheritdoc />
        public async Task<Stream> CreateIcalForEventAsync(Guid eventId, DateTime? createTime = null)
        {
            var evt = await eventRepository.FindByIdAsync(eventId);
            if (evt == null)
            {
                throw new InvalidOperationException($"Event with ID [{eventId}] not found!");
            }

            return CreateIcalStream(evt, createTime);
        }

        /// <inheritdoc />
        public async Task<Event> UpsertEventAsync(Event eventToUpsert, Diver currentUser)
        {
            if (eventToUpsert == null) throw new ArgumentNullException(nameof(eventToUpsert));

            Event eventToStore;
            var isNew = eventToUpsert.Id == Guid.Empty;
            if (!isNew)
            {
                eventToStore = await eventRepository.FindByIdAsync(eventToUpsert.Id);

                if (eventToStore == null)
                {
                    throw new InvalidOperationException("Aktivität zum bearbeiten nicht in der Datenbank gefunden!");
                }
            }
            else
            {
                eventToStore = new Event { Id = Guid.NewGuid() };
                eventToStore.OrganisatorId = eventToUpsert.OrganisatorId;
            }

            eventToStore.Name = eventToUpsert.Name;
            eventToStore.Description = eventToUpsert.Description;
            eventToStore.Location = eventToUpsert.Location;
            eventToStore.MeetingPoint = eventToUpsert.MeetingPoint;
            eventToStore.StartTime = eventToUpsert.StartTime;
            eventToStore.EndTime = eventToUpsert.EndTime;

            if (isNew)
            {
                await eventRepository.InsertAsync(eventToStore);
                await notificationService.NotifyForNewEventAsync(eventToStore, currentUser);
                TrackEvent("EVENT-INSERT", eventToStore);
            }
            else
            {
                eventRepository.Update(eventToStore);
                await notificationService.NotifyForChangedEventAsync(eventToStore, currentUser);
                TrackEvent("EVENT-UPDATE", eventToStore);
            }

            return eventToStore;
        }

        /// <inheritdoc/>
        public async Task<Comment> AddCommentAsync(Guid eventId, string commentToAdd, Diver authorDiver)
        {
            if (eventId == Guid.Empty) { throw new ArgumentException("Empty Guid not allowed as Event-Id!", nameof(eventId)); }
            if (authorDiver == null) throw new ArgumentNullException(nameof(authorDiver));

            if (!string.IsNullOrWhiteSpace(commentToAdd))
            {
                var evt = await eventRepository.FindByIdAsync(eventId);
                if (evt == null)
                {
                    throw new InvalidOperationException($"Event with Id [{eventId.ToString("G")}] not found!");
                }

                var comment = new Comment
                {
                    Id = Guid.NewGuid(),
                    AuthorId = authorDiver.Id,
                    CreateDate = DateTime.Now,
                    EventId = evt.Id,
                    Text = commentToAdd,
                };

                await commentRepository.InsertAsync(comment);
                await notificationService.NotifyForEventCommentAsync(comment, evt, authorDiver);
                TrackEvent("COMMENT-INSERT", comment);

                return comment;
            }

            return null;
        }

        /// <inheritdoc/>
        public async Task<Comment> EditCommentAsync(Guid commentId, string commentText, Diver currentUser)
        {
            if (commentId == Guid.Empty) { throw new ArgumentException("Guid.Empty not allowed!", nameof(commentId)); }
            if (currentUser == null) { throw new ArgumentNullException(nameof(currentUser)); }

            var comment = await commentRepository.FindByIdAsync(commentId);
            if (comment == null)
            {
                throw new InvalidOperationException($"Could not find comment with ID [{commentId:D}]");
            }

            if (comment.AuthorId != currentUser.Id)
            {
                throw new UnauthorizedAccessException();
            }

            comment.Text = commentText;

            var parentEvent = await eventRepository.FindByIdAsync(comment.EventId);
            if (parentEvent == null)
            {
                throw new InvalidOperationException($"Could not find event for comment with ID=[{comment.EventId:D}]");
            }

            await notificationService.NotifyForEventCommentAsync(comment, parentEvent, currentUser);
            TrackEvent("COMMENT-UPDATE", comment);

            return comment;
        }

        /// <inheritdoc />
        public async Task DeleteCommentAsync(Guid commentId, Diver currentUser)
        {
            if (commentId == Guid.Empty) { throw new ArgumentException("Guid.Empty not allowed!", nameof(commentId)); }
            if (currentUser == null) throw new ArgumentNullException(nameof(currentUser));

            var comment = await commentRepository.FindByIdAsync(commentId);
            if (comment == null)
            {
                throw new InvalidOperationException($"Could not find comment with ID [{commentId:D}] for deletion!");
            }

            if (comment.AuthorId != currentUser.Id)
            {
                throw new UnauthorizedAccessException();
            }

            TrackEvent("COMMENT-DELETE", comment);
            commentRepository.Delete(comment);
        }

        /// <inheritdoc />
        public async Task<ICollection<Event>> GetUpcomingAndRecentEventsAsync()
            => await eventRepository.GetUpcomingAndRecentEventsAsync();

        /// <inheritdoc />
        public async Task<ICollection<Event>> GetUpcomingEventsAsync()
            => await eventRepository.GetUpcomingEventsAsync();

        /// <inheritdoc />
        public async Task<Event> GetByIdAsync(Guid eventId)
            => await eventRepository.FindByIdAsync(eventId);

        private Stream CreateIcalStream(Event evt, DateTimeOffset? createTime = null)
        {
            var sb = new StringBuilder();
            const string dateFormat = "yyyyMMddTHHmmssZ";
            var createAt = createTime != null
                ? createTime.Value
                : DateTimeOffset.Now;
            var createAtString = createAt.ToString(dateFormat);

            sb.AppendLine("BEGIN:VCALENDAR");
            sb.AppendLine("PRODID:-//Tauchbolde//TauchboldeWebsite//EN");
            sb.AppendLine("VERSION:2.0");
            sb.AppendLine("METHOD:PUBLISH");


            var evtEndTime = evt.EndTime ?? evt.StartTime.AddHours(4);
            var dtStart = evt.StartTime.ToLocalTime();
            var dtEnd = evtEndTime.ToLocalTime();

            sb.AppendLine("BEGIN:VEVENT");
            sb.AppendLine("DTSTART:" + dtStart.ToString(dateFormat));
            sb.AppendLine("DTEND:" + dtEnd.ToString(dateFormat));
            sb.AppendLine("DTSTAMP:" + createAtString);
            sb.AppendLine("UID:" + evt.Id);
            sb.AppendLine("CREATED:" + createAtString);
            sb.AppendLine("X-ALT-DESC;FMTTYPE=text/html:" + evt.Description);
            sb.AppendLine("DESCRIPTION:" + evt.Description);
            sb.AppendLine("LAST-MODIFIED:" + createAtString);
            sb.AppendLine("LOCATION:" + evt.Location);
            sb.AppendLine("SEQUENCE:0");
            sb.AppendLine("STATUS:CONFIRMED");
            sb.AppendLine("SUMMARY:" + evt.Name);
            sb.AppendLine("TRANSP:OPAQUE");
            sb.AppendLine("END:VEVENT");
            sb.AppendLine("END:VCALENDAR");

            var calendarBytes = Encoding.UTF8.GetBytes(sb.ToString());

            TrackEvent("EVENT-ICAL", evt);

            return new MemoryStream(calendarBytes);
        }

        private void TrackEvent(string name, Event eventToTrack)
        {
            if (eventToTrack == null) { throw new ArgumentNullException(nameof(eventToTrack)); }

            telemetryService.TrackEvent(
                name,
                new Dictionary<string, string>
                {
                    {"EventId", eventToTrack.Id.ToString("B")},
                    {"EventName", eventToTrack.Name},
                    {"StartEnd", eventToTrack.StartEndTimeAsString},
                    {"OrganisatorId", eventToTrack.OrganisatorId.ToString("B")}
                });
        }

        private void TrackEvent(string name, Comment commentToTrack)
        {
            if (commentToTrack == null) { throw new ArgumentNullException(nameof(commentToTrack)); }

            telemetryService.TrackEvent(
                name,
                new Dictionary<string, string>
                {
                    {"EventId", commentToTrack.EventId.ToString("B")},
                    {"CommentId", commentToTrack.Id.ToString("B")},
                    {"AuthorId", commentToTrack.AuthorId.ToString("B")},
                    {"Text", commentToTrack.Text},
                    {"CreateDate", commentToTrack.CreateDate.ToString("O")},
                    {"ModifiedDate", commentToTrack.ModifiedDate?.ToString("O") ?? ""}
                });
        }
    }
}
