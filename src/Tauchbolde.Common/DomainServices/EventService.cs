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
        private readonly INotificationService notificationService;

        public EventService(
            ApplicationDbContext applicationDbContext,
            INotificationService notificationService)
        {
            if (applicationDbContext == null) throw new ArgumentNullException(nameof(applicationDbContext));

            _applicationDbContext = applicationDbContext;
            this.notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
        }

        /// <inheritdoc />
        public async Task<Stream> CreateIcalForEventAsync(Guid eventId, IEventRepository eventRepository)
        {
            if (eventRepository == null) throw new ArgumentNullException(nameof(eventRepository));

            var evt = await eventRepository.FindByIdAsync(eventId);
            if (evt == null)
            {
                throw new InvalidOperationException($"Event with ID [{eventId}] not found!");
            }
            
            return CreateIcalStream(evt);
        }

        /// <inheritdoc />
        public async Task<Event> UpsertEventAsync(IEventRepository eventRepository, Event eventToUpsert)
        {
            if (eventRepository == null) throw new ArgumentNullException(nameof(eventRepository));
            if (eventToUpsert == null) throw new ArgumentNullException(nameof(eventToUpsert));

            Event eventToStore = null;
            bool isNew = eventToUpsert.Id == Guid.Empty;
            if (eventToUpsert.Id != Guid.Empty)
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
                await notificationService.NotifyForNewEventAsync(eventToStore);
            }
            else
            {
                eventRepository.Update(eventToStore);
                await notificationService.NotifyForChangedEventAsync(eventToStore);
            }

            return eventToStore;
        }

        /// <inheritdoc/>
        public async Task<Comment> AddCommentAsync(Guid eventId, string commentToAdd, Diver authorDiver, ICommentRepository commentRepository)
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

                await commentRepository.InsertAsync(comment);
                await notificationService.NotifyForEventCommentAsync(comment);

                return comment;
            }

            return null;
        }

        /// <inheritdoc/>
        public async Task<Comment> EditCommentAsync(Guid commentId, string commentText, Diver currentUser, ICommentRepository commentRepository)
        {
            if (commentId == Guid.Empty) { throw new ArgumentException("Guid.Empty not allowed!", nameof(commentId)); }
            if (currentUser == null) { throw new ArgumentNullException(nameof(currentUser)); }
            if (commentRepository == null) { throw new ArgumentNullException(nameof(commentRepository)); }

            var comment = await commentRepository.FindByIdAsync(commentId);
            if (comment != null) {
                if (comment.AuthorId != currentUser.Id)
                {
                    throw new UnauthorizedAccessException();
                }

                comment.Text = commentText;
            }

            await notificationService.NotifyForEventCommentAsync(comment);

            return comment;
        }

        public async Task DeleteCommentAsync(Guid commentId, Diver currentUser, ICommentRepository commentRepository)
        {
            if (commentId == Guid.Empty) { throw new ArgumentException("Guid.Empty not allowed!", nameof(commentId)); }
            if (currentUser == null) throw new ArgumentNullException(nameof(currentUser));
            if (commentRepository == null) throw new ArgumentNullException(nameof(commentRepository));

            var comment = await commentRepository.FindByIdAsync(commentId);
            if (comment != null)
            {
                if (comment.AuthorId != currentUser.Id)
                {
                    throw new UnauthorizedAccessException();
                }

                commentRepository.Delete(comment);
            }
        }

        private static Stream CreateIcalStream(Event evt)
        {
            var sb = new StringBuilder();
            const string dateFormat = "yyyyMMddTHHmmssZ";
            var now = DateTime.Now.ToUniversalTime().ToString(dateFormat);

            sb.AppendLine("BEGIN:VCALENDAR");
            sb.AppendLine("PRODID:-//Tauchbolde//TauchboldeWebsite//EN");
            sb.AppendLine("VERSION:2.0");
            sb.AppendLine("METHOD:PUBLISH");


            var evtEndTime = evt.EndTime ?? evt.StartTime.AddHours(4);
            var dtStart = Convert.ToDateTime(evt.StartTime);
            var dtEnd = Convert.ToDateTime(evtEndTime);

            sb.AppendLine("BEGIN:VEVENT");
            sb.AppendLine("DTSTART:" + dtStart.ToUniversalTime().ToString(dateFormat));
            sb.AppendLine("DTEND:" + dtEnd.ToUniversalTime().ToString(dateFormat));
            sb.AppendLine("DTSTAMP:" + now);
            sb.AppendLine("UID:" + evt.Id);
            sb.AppendLine("CREATED:" + now);
            sb.AppendLine("X-ALT-DESC;FMTTYPE=text/html:" + evt.Description);
            sb.AppendLine("DESCRIPTION:" + evt.Description);
            sb.AppendLine("LAST-MODIFIED:" + now);
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
