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
        [NotNull] private readonly INotificationRepository notificationRepository;

        public NotificationPublisher(
            [NotNull] IEventRepository eventRepository,
            [NotNull] ILogbookEntryRepository logbookEntryRepository,
            [NotNull] INotificationRepository notificationRepository)
        {
            this.eventRepository = eventRepository ?? throw new ArgumentNullException(nameof(eventRepository));
            this.logbookEntryRepository = logbookEntryRepository ?? throw new ArgumentNullException(nameof(logbookEntryRepository));
            this.notificationRepository = notificationRepository ?? throw new ArgumentNullException(nameof(notificationRepository));
        }

        public async Task PublishAsync(
            NotificationType notificationType,
            string message,
            IEnumerable<Diver> recipients,
            Diver currentDiver = null,
            Guid? relatedEventId = null,
            Guid? relatedLogbookEntryId = null)
        {
            if (recipients == null)
            {
                return;
            }

            var relatedEvent = relatedEventId != null
                ? await eventRepository.FindByIdAsync(relatedEventId.Value)
                : null;

            var relatedLogbookEntry = relatedLogbookEntryId != null
                ? await logbookEntryRepository.FindByIdAsync(relatedLogbookEntryId.Value)
                : null;

            foreach (var recipient in GetRelevantRecipients(recipients, currentDiver))
            {
                await notificationRepository.InsertAsync(
                    new Notification(
                        recipient,
                        notificationType,
                        message,
                        relatedEvent,
                        relatedLogbookEntry));
            }
        }

        private static IEnumerable<Diver> GetRelevantRecipients(IEnumerable<Diver> recipients, Diver currentDiver) =>
            recipients
                .Where(r =>
                    currentDiver == null ||
                    currentDiver.Id == r.Id ||
                    currentDiver.SendOwnNoticiations);
    }
}