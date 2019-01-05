using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Tauchbolde.Common.Model;
using Tauchbolde.Common.Repositories;
using Tauchbolde.Common.DomainServices.Notifications;

namespace Tauchbolde.Common.DomainServices
{
    public class EventService : IEventService
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly INotificationService _notificationService;
        private readonly IEventRepository _eventRepository;
        private readonly ICommentRepository _commentRepository;

        public EventService(
            ApplicationDbContext applicationDbContext,
            INotificationService notificationService,
            IEventRepository eventRepository,
            ICommentRepository commentRepository)
        {
            _applicationDbContext = applicationDbContext ?? throw new ArgumentNullException(nameof(applicationDbContext));
            _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
            _eventRepository = eventRepository ?? throw new ArgumentNullException(nameof(eventRepository));
            _commentRepository = commentRepository ?? throw new ArgumentNullException(nameof(commentRepository));
        }

        /// <inheritdoc />
        public async Task<Stream> CreateIcalForEventAsync(Guid eventId, DateTime? createTime = null)
        {
            var evt = await _eventRepository.FindByIdAsync(eventId);
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

            Event eventToStore = null;
            bool isNew = eventToUpsert.Id == Guid.Empty;
            if (!isNew)
            {
                eventToStore = await _eventRepository.FindByIdAsync(eventToUpsert.Id);

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
                await _eventRepository.InsertAsync(eventToStore);
                await _notificationService.NotifyForNewEventAsync(eventToStore, currentUser);
            }
            else
            {
                _eventRepository.Update(eventToStore);
                await _notificationService.NotifyForChangedEventAsync(eventToStore, currentUser);
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
                var comment = new Comment
                {
                    Id = Guid.NewGuid(),
                    AuthorId = authorDiver.Id,
                    CreateDate = DateTime.Now,
                    EventId = eventId,
                    Text = commentToAdd,
                };

                await _commentRepository.InsertAsync(comment);
                await _notificationService.NotifyForEventCommentAsync(comment, authorDiver);

                return comment;
            }

            return null;
        }

        /// <inheritdoc/>
        public async Task<Comment> EditCommentAsync(Guid commentId, string commentText, Diver currentUser)
        {
            if (commentId == Guid.Empty) { throw new ArgumentException("Guid.Empty not allowed!", nameof(commentId)); }
            if (currentUser == null) { throw new ArgumentNullException(nameof(currentUser)); }

            var comment = await _commentRepository.FindByIdAsync(commentId);
            if (comment != null) {
                if (comment.AuthorId != currentUser.Id)
                {
                    throw new UnauthorizedAccessException();
                }

                comment.Text = commentText;
            }

            await _notificationService.NotifyForEventCommentAsync(comment, currentUser);

            return comment;
        }

        public async Task DeleteCommentAsync(Guid commentId, Diver currentUser)
        {
            if (commentId == Guid.Empty) { throw new ArgumentException("Guid.Empty not allowed!", nameof(commentId)); }
            if (currentUser == null) throw new ArgumentNullException(nameof(currentUser));

            var comment = await _commentRepository.FindByIdAsync(commentId);
            if (comment != null)
            {
                if (comment.AuthorId != currentUser.Id)
                {
                    throw new UnauthorizedAccessException();
                }

                _commentRepository.Delete(comment);
            }
        }

        private static Stream CreateIcalStream(Event evt, DateTimeOffset? createTime = null)
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
            //}

            sb.AppendLine("END:VCALENDAR");

            var calendarBytes = Encoding.UTF8.GetBytes(sb.ToString());

            return new MemoryStream(calendarBytes);
        }
    }
}
