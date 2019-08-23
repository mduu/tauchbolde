using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.Application.OldDomainServices.Notifications;
using Tauchbolde.Application.Services.Telemetry;
using Tauchbolde.Domain.Entities;

namespace Tauchbolde.Application.OldDomainServices.Events
{
    internal class EventService : IEventService
    {
        private readonly INotificationService notificationService;
        private readonly IEventRepository eventRepository;
        private readonly ITelemetryService telemetryService;

        public EventService(
            INotificationService notificationService,
            IEventRepository eventRepository,
            ITelemetryService telemetryService)
        {
            this.notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
            this.eventRepository = eventRepository ?? throw new ArgumentNullException(nameof(eventRepository));
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
                eventRepository.UpdateAsync(eventToStore);
                await notificationService.NotifyForChangedEventAsync(eventToStore, currentUser);
                TrackEvent("EVENT-UPDATE", eventToStore);
            }

            return eventToStore;
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
    }
}
