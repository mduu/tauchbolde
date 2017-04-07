using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Tauchbolde.Common.Model;
using Tauchbolde.Common.Repositories;

namespace Tauchbolde.Common.DomainServices
{
    public class EventService : IEventService
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public EventService(ApplicationDbContext applicationDbContext)
        {
            if (applicationDbContext == null) throw new ArgumentNullException(nameof(applicationDbContext));

            _applicationDbContext = applicationDbContext;
        }

        /// <inheritdoc />
        public async Task<Stream> CreateIcalForEvent(Guid eventId, IEventRepository eventRepository)
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
        public async Task<Event> UpdateEventAsync(IEventRepository eventRepository, Guid eventId, string name, string description,
            DateTime startTime, DateTime? endTime, string location, string meetingPoint, ApplicationUser currentUser)
        {
            if (eventRepository == null) throw new ArgumentNullException(nameof(eventRepository));
            if (eventId == Guid.Empty) throw new ArgumentException("EventId can not be empty!", nameof(eventId));

            var evt = await eventRepository.FindByIdAsync(eventId);
            if (evt == null)
            {
                throw new InvalidOperationException("Zu bearbeitendes Event wurde nicht in der Datenbank gefunden!");
            }

            evt.Name = name;
            evt.Description = description;
            evt.Location = location;
            evt.MeetingPoint = meetingPoint;
            evt.StartTime = startTime;
            evt.EndTime = endTime;



            throw new NotImplementedException();
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
            }

            eventToStore.Name = eventToUpsert.Name;
            eventToStore.Description = eventToUpsert.Description;
            eventToStore.Organisator = eventToUpsert.Organisator;
            eventToStore.Location = eventToUpsert.Location;
            eventToStore.MeetingPoint = eventToUpsert.MeetingPoint;
            eventToStore.StartTime = eventToUpsert.StartTime;
            eventToStore.EndTime = eventToUpsert.EndTime;

            if (isNew)
            {
                await eventRepository.InsertAsync(eventToStore);
            }
            else
            {
                eventRepository.Update(eventToStore);
            }

            return eventToStore;
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
