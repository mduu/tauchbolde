using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Tauchbolde.Common.Domain.Repositories;
using Tauchbolde.Common.Infrastructure.Telemetry;
using Tauchbolde.Common.Model;

namespace Tauchbolde.Common.Domain.Notifications
{
    /// <summary>
    /// The notification service allows the app to register notifications to be send somewhen later on.
    /// </summary>
    internal class NotificationService : INotificationService
    {
        [NotNull] private readonly INotificationRepository notificationRepository;
        [NotNull] private readonly IDiverRepository diverRepository;
        [NotNull] private readonly IParticipantRepository participantRepository;
        [NotNull] private readonly ITelemetryService telemetryService;

        public NotificationService(
            [NotNull] INotificationRepository notificationRepository,
            [NotNull] IDiverRepository diverRepository,
            [NotNull] IParticipantRepository participantRepository,
            [NotNull] ITelemetryService telemetryService)
        {
            this.notificationRepository = notificationRepository ?? throw new ArgumentNullException(nameof(notificationRepository));
            this.diverRepository = diverRepository ?? throw new ArgumentNullException(nameof(diverRepository));
            this.participantRepository = participantRepository ?? throw new ArgumentNullException(nameof(participantRepository));
            this.telemetryService = telemetryService ?? throw new ArgumentNullException(nameof(telemetryService));
        }

        /// <inheritdoc />
        public async Task NotifyForNewEventAsync(
            [NotNull] Event newEvent,
            [CanBeNull] Diver currentUser)
        {
            if (newEvent == null) throw new ArgumentNullException(nameof(newEvent));

            var recipients = await diverRepository.GetAllTauchboldeUsersAsync();
            var message = $"Neue Aktivität '{newEvent.Name}' ({newEvent.StartEndTimeAsString}) von {newEvent.Organisator.Realname} erstellt";

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
            var message = $"Aktivität geändert '{changedEvent.Name}' ({changedEvent.StartEndTimeAsString}) von {changedEvent.Organisator.Realname}";

            await InsertNotification(
                changedEvent,
                recipients,
                NotificationType.EditEvent,
                message,
                currentUser);
        }

        /// <inheritdoc />
        public async Task NotifyForCanceledEventAsync(
            [NotNull] Event canceledEvent,
            [CanBeNull] Diver currentUser)
        {
            if (canceledEvent == null) throw new ArgumentNullException(nameof(canceledEvent));

            // Recipients all Tauchbolde that did not yet "decline" the Event already
            var declinedParticipants = await participantRepository.GetParticipantsForEventByStatusAsync(canceledEvent.Id, ParticipantStatus.Declined);
            var recipients = (await diverRepository.GetAllTauchboldeUsersAsync())
                .Where(u => declinedParticipants.All(p => p.ParticipatingDiver.Id != u.Id))
                .ToList();

            var message = $"Aktivität '{canceledEvent.Name}' ({canceledEvent.StartEndTimeAsString}) wurde abgesagt von {canceledEvent.Organisator.Realname}.";

            await InsertNotification(
                canceledEvent,
                recipients,
                NotificationType.CancelEvent,
                message,
                currentUser);
        }

        /// <inheritdoc />
        public async Task NotifyForChangedParticipationAsync(
            [NotNull] Participant participant,
            [NotNull] Diver participatingDiver,
            [NotNull] Event participatingEvent)
        {
            if (participant == null) throw new ArgumentNullException(nameof(participant));
            if (participatingDiver == null) throw new ArgumentNullException(nameof(participatingDiver));
            if (participatingEvent == null) throw new ArgumentNullException(nameof(participatingEvent));

            var recipients = await GetAllTauchboldeButDeclinedParticipantsAsync(participatingDiver.Id, participant.EventId);

            string message;
            NotificationType notificationType;
            switch (participant.Status)
            {
                case ParticipantStatus.None:
                    message = $"{participatingDiver.Realname ?? "Unbekannt"} weiss nicht ob Er/Sie an der Aktivität '{participatingEvent.Name}' ({participatingEvent.StartEndTimeAsString}) teil nimmt.";
                    notificationType = NotificationType.Neutral;
                    break;
                case ParticipantStatus.Accepted:
                    message = $"{participatingDiver.Realname ?? "Unbekannt"} nimmt an der Aktivität '{participatingEvent.Name}' ({participatingEvent.StartEndTimeAsString}) teil.";
                    notificationType = NotificationType.Accepted;
                    break;
                case ParticipantStatus.Declined:
                    message = $"{participatingDiver.Realname ?? "Unbekannt"} hat für die Aktivität '{participatingEvent.Name}' ({participatingEvent.StartEndTimeAsString}) abgesagt.";
                    notificationType = NotificationType.Declined;
                    break;
                case ParticipantStatus.Tentative:
                    message = $"{participatingDiver.Realname ?? "Unbekannt"} nimmt eventuell an der Aktivität '{participatingEvent.Name}' ({participatingEvent.StartEndTimeAsString}) teil.";
                    notificationType = NotificationType.Tentative;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            if (!string.IsNullOrWhiteSpace(participant.Note))
            {
                message = $"{message}: {participant.Note}";
            }

            await InsertNotification(
                participatingEvent,
                recipients,
                notificationType,
                message,
                participant.ParticipatingDiver);
        }

        /// <inheritdoc />
        public async Task NotifyForEventCommentAsync(
            [NotNull] Comment comment,
            [NotNull] Event evt,
            [NotNull] Diver author)
        {
            if (comment == null) throw new ArgumentNullException(nameof(comment));
            if (evt == null) throw new ArgumentNullException(nameof(evt));
            if (author == null) throw new ArgumentNullException(nameof(author));

            var recipients = await GetAllTauchboldeButDeclinedParticipantsAsync(author.Id, comment.EventId);
            var message = $"Neuer Kommentar von '{author.Realname}' für Event '{evt.Name}' ({evt.StartEndTimeAsString}): {comment.Text}";

            await InsertNotification(
                evt,
                recipients,
                NotificationType.Commented,
                message,
                author);
        }

        /// <inheritdoc />
        public async Task NotifyForNewLogbookEntry(
            [NotNull] LogbookEntry logbookEntry,
            [NotNull] Diver author)
        {
            if (logbookEntry == null) throw new ArgumentNullException(nameof(logbookEntry));
            if (author == null) throw new ArgumentNullException(nameof(author));

            var recipients = await diverRepository.GetAllTauchboldeUsersAsync();
            var message = $"Neuer Logbucheintrag '{logbookEntry.Title} von {author.Realname}.";

            await InsertNotification(
                null,
                recipients,
                NotificationType.NewLogbookEntry,
                message,
                author,
                logbookEntry);
        }

        private async Task<List<Diver>> GetAllTauchboldeButDeclinedParticipantsAsync(
            Guid currentDiverId,
            Guid eventId)
        {
            var declinedParticipants =
                (await participantRepository.GetParticipantsForEventByStatusAsync(eventId, ParticipantStatus.Declined))
                .Where(p => p.ParticipatingDiver.Id != currentDiverId);

            var result = (await diverRepository
                .GetAllTauchboldeUsersAsync())
                .Where(u =>
                    declinedParticipants.All(p => p.ParticipatingDiver.Id != u.Id))
                .ToList();

            return result;
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
                    { "NotificationId", notification.Id.ToString("B") },
                    { "RecipientId", notification.Recipient?.Id.ToString("B") ?? "" },
                    { "Type", notification.Type.ToString() },
                    { "Message", notification.Message },
                });
        }
    }
}