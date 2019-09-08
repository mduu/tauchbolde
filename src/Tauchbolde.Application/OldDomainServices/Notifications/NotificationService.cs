using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.Application.Services.Telemetry;
using Tauchbolde.Domain.Entities;
using Tauchbolde.Domain.Types;

namespace Tauchbolde.Application.OldDomainServices.Notifications
{
    /// <summary>
    /// The notification service allows the app to register notifications to be send somewhen later on.
    /// </summary>
    internal class NotificationService : INotificationService
    {
        [NotNull] private readonly INotificationRepository notificationRepository;
        [NotNull] private readonly IDiverRepository diverRepository;
        [NotNull] private readonly ITelemetryService telemetryService;

        public NotificationService(
            [NotNull] INotificationRepository notificationRepository,
            [NotNull] IDiverRepository diverRepository,
            [NotNull] ITelemetryService telemetryService)
        {
            this.notificationRepository = notificationRepository ?? throw new ArgumentNullException(nameof(notificationRepository));
            this.diverRepository = diverRepository ?? throw new ArgumentNullException(nameof(diverRepository));
            this.telemetryService = telemetryService ?? throw new ArgumentNullException(nameof(telemetryService));
        }

        /// <inheritdoc />
        public async Task NotifyForNewEventAsync(
            [NotNull] Event newEvent,
            [CanBeNull] Diver currentUser)
        {
            if (newEvent == null) throw new ArgumentNullException(nameof(newEvent));

            var recipients = await diverRepository.GetAllTauchboldeUsersAsync();
            var message =
                $"Neue Aktivität '{newEvent.Name}' ({newEvent.StartEndTimeAsString}) von {newEvent.Organisator.Realname} erstellt";

            await InsertNotification(
                newEvent,
                recipients,
                NotificationType.NewEvent,
                message,
                currentUser);
        }

        /// <inheritdoc />
        public async Task NotifyForChangedEventAsync(
            [NotNull] Event changedEvent,
            [CanBeNull] Diver currentUser)
        {
            if (changedEvent == null) throw new ArgumentNullException(nameof(changedEvent));

            var recipients = await diverRepository.GetAllTauchboldeUsersAsync();
            var message =
                $"Aktivität geändert '{changedEvent.Name}' ({changedEvent.StartEndTimeAsString}) von {changedEvent.Organisator.Realname}";

            await InsertNotification(
                changedEvent,
                recipients,
                NotificationType.EditEvent,
                message,
                currentUser);
        }

        private async Task InsertNotification(
            [CanBeNull] Event relatedEvent,
            [NotNull] IEnumerable<Diver> recipients,
            NotificationType notificationType,
            string message,
            [CanBeNull] Diver currentDiver = null,
            [CanBeNull] LogbookEntry relatedLogbookEntry = null)
        {
            if (recipients == null) throw new ArgumentNullException(nameof(recipients));

            foreach (var recipient in recipients)
            {
                if (currentDiver != null && !currentDiver.SendOwnNoticiations &&
                    currentDiver.Id == recipient.Id)
                {
                    continue;
                }

                var notification = new Notification
                {
                    Id = Guid.NewGuid(),
                    Event = relatedEvent,
                    OccuredAt = DateTime.Now,
                    Recipient = recipient,
                    Type = notificationType,
                    Message = message,
                    LogbookEntry = relatedLogbookEntry,
                };

                await notificationRepository.InsertAsync(notification);

                TrackEvent("NOTIFICATION-INSERT", notification);
            }
        }

        private void TrackEvent([NotNull] string name, [NotNull] Notification notification)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
            if (notification == null) throw new ArgumentNullException(nameof(notification));

            telemetryService.TrackEvent(
                name,
                new Dictionary<string, string>
                {
                    {"NotificationId", notification.Id.ToString("B")},
                    {"RecipientId", notification.Recipient?.Id.ToString("B") ?? ""},
                    {"Type", notification.Type.ToString()},
                    {"Message", notification.Message},
                });
        }
    }
}