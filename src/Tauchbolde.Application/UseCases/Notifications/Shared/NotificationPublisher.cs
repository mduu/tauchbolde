using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.Domain.Entities;
using Tauchbolde.Domain.Types;

namespace Tauchbolde.Application.UseCases.Notifications.Shared
{
    [UsedImplicitly]
    public class NotificationPublisher : INotificationPublisher
    {
        [NotNull] private readonly INotificationRepository dataAccess;

        public NotificationPublisher([NotNull] INotificationRepository dataAccess)
        {
            this.dataAccess = dataAccess ?? throw new ArgumentNullException(nameof(dataAccess));
        }

        public async Task PublishAsync(NotificationType notificationType,
            string message,
            IEnumerable<Diver> recipients,
            Event relatedEvent = null,
            Diver currentDiver = null,
            LogbookEntry relatedLogbookEntry = null)
        {
            if (recipients == null)
            {
                return;
            }

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
                TrackEvent("NOTIFICATION-INSERT", newNotification);
            }
        }

        private void TrackEvent(string notificationInsert, Notification newNotification)
        {
            throw new NotImplementedException();
        }

        private static IEnumerable<Diver> GetRelevantRecipients(IEnumerable<Diver> recipients, Diver currentDiver) =>
            recipients
                .Where(r => currentDiver != null &&
                            !currentDiver.SendOwnNoticiations &&
                            currentDiver.Id == r.Id);
    }
}