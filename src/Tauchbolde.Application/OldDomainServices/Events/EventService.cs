using System;
using System.Collections.Generic;
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
                await eventRepository.UpdateAsync(eventToStore);
                await notificationService.NotifyForChangedEventAsync(eventToStore, currentUser);
                TrackEvent("EVENT-UPDATE", eventToStore);
            }

            return eventToStore;
        }

        /// <inheritdoc />
        public async Task<Event> GetByIdAsync(Guid eventId)
            => await eventRepository.FindByIdAsync(eventId);

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
