using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.Domain.Entities;
using Tauchbolde.Domain.Types;

namespace Tauchbolde.Application.Services.Notifications
{
    [UsedImplicitly]
    public class NotificationPublisher : INotificationPublisher
    {
        [NotNull] private readonly IEventRepository eventRepository;
        [NotNull] private readonly ILogbookEntryRepository logbookEntryRepository;
        [NotNull] private readonly INotificationRepository dataAccess;

        public NotificationPublisher(
            [NotNull] IEventRepository eventRepository,
            [NotNull] ILogbookEntryRepository logbookEntryRepository,
            [NotNull] INotificationRepository dataAccess)
        {
            this.eventRepository = eventRepository ?? throw new ArgumentNullException(nameof(eventRepository));
            this.logbookEntryRepository = logbookEntryRepository ?? throw new ArgumentNullException(nameof(logbookEntryRepository));
            this.dataAccess = dataAccess ?? throw new ArgumentNullException(nameof(dataAccess));
        }

        public async Task PublishAsync(
            NotificationType notificationType,
            string message,
            IEnumerable<Diver> recipients,
            Diver currentDiver = null,
            Guid? relatedEventId = null,
            Guid? relatedLogbookEntryId = null)
        {
            if (recipients == null) { return; }
            
            var relatedEvent = relatedEventId != null
                ? await eventRepository.FindByIdAsync(relatedEventId.Value)
                : null;
            
            var relatedLogbookEntry = relatedLogbookEntryId != null
                ? await logbookEntryRepository.FindByIdAsync(relatedLogbookEntryId.Value)
                : null;

            foreach (var recipient in GetRelevantRecipients(recipients, currentDiver))
            {
                var newNotification = new Notification
                {
                    Id = Guid.NewGuid(),
                    Event = relatedEvent,
                    OccuredAt = DateTime.Now,
                    Recipient = recipient,
                    Type = notificationType,
                    Message = message,
                    LogbookEntry = relatedLogbookEntry,
                };
                
                await dataAccess.InsertAsync(newNotification);
            }
        }

        private static IEnumerable<Diver> GetRelevantRecipients(IEnumerable<Diver> recipients, Diver currentDiver) =>
            recipients
                .Where(r => currentDiver != null &&
                            !currentDiver.SendOwnNoticiations &&
                            currentDiver.Id == r.Id);
    }
}