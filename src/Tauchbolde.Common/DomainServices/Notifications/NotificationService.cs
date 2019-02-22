using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tauchbolde.Common.Model;
using Tauchbolde.Common.Repositories;
using Tauchbolde.Common.Infrastructure.Telemetry;

namespace Tauchbolde.Common.DomainServices.Notifications
{
    /// <summary>
    /// The notification service allows the app to register notifications to be send somewhen later on.
    /// </summary>
    internal class NotificationService : INotificationService
    {
        private readonly INotificationRepository notificationRepository;
        private readonly IDiverRepository diverRepository;
        private readonly IParticipantRepository participantRepository;
        private readonly ITelemetryService telemetryService;

        public NotificationService(
            INotificationRepository notificationRepository,
            IDiverRepository diverRepository,
            IParticipantRepository participantRepository,
            ITelemetryService telemetryService)
        {
            this.notificationRepository = notificationRepository ?? throw new ArgumentNullException(nameof(notificationRepository));
            this.diverRepository = diverRepository ?? throw new ArgumentNullException(nameof(diverRepository));
            this.participantRepository = participantRepository ?? throw new ArgumentNullException(nameof(participantRepository));
            this.telemetryService = telemetryService ?? throw new ArgumentNullException(nameof(telemetryService));
        }

        /// <inheritdoc />
        public async Task NotifyForNewEventAsync(
            Event newEvent,
            Diver currentUser)
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
            Event changedEvent,
            Diver currentUser)
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
            Event canceledEvent,
            Diver currentUser)
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
        public async Task NotifyForChangedParticipationAsync(Participant participant)
        {
            if (participant == null) throw new ArgumentNullException(nameof(participant));

            var recipients = await GetAllTauchboldeButDeclinedParticipantsAsync(participant.ParticipatingDiver.Id, participant.EventId);

            var message = "";
            NotificationType notificationType;
            switch (participant.Status)
            {
                case ParticipantStatus.None:
                    message = $"Teilnahme: {participant?.ParticipatingDiver?.Realname ?? "Unbekannt"} weiss nicht ob Er/Sie an der Aktivität '{participant.Event.Name}' ({participant.Event.StartEndTimeAsString}) teil nimmt.";
                    notificationType = NotificationType.Neutral;
                    break;
                case ParticipantStatus.Accepted:
                    message = $"Teilnahme: {participant?.ParticipatingDiver?.Realname ?? "Unbekannt"} nimmt an der Aktivität '{participant.Event.Name}' ({participant.Event.StartEndTimeAsString}) teil.";
                    notificationType = NotificationType.Accepted;
                    break;
                case ParticipantStatus.Declined:
                    message = $"Teilnahme: {participant?.ParticipatingDiver?.Realname ?? "Unbekannt"} hat für die Aktivität '{participant.Event.Name}' ({participant.Event.StartEndTimeAsString}) abgesagt.";
                    notificationType = NotificationType.Declined;
                    break;
                case ParticipantStatus.Tentative:
                    message = $"Teilnahme: {participant?.ParticipatingDiver?.Realname ?? "Unbekannt"} nimmt eventuell an der Aktivität '{participant.Event.Name}' ({participant.Event.StartEndTimeAsString}) teil.";
                    notificationType = NotificationType.Tentative;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            await InsertNotification(
                participant.Event,
                recipients,
                notificationType,
                message,
                participant.ParticipatingDiver);
        }

        /// <inheritdoc />
        public async Task NotifyForEventCommentAsync(Comment comment, Diver author)
        {
            if (comment == null) throw new ArgumentNullException(nameof(comment));

            var recipients = await GetAllTauchboldeButDeclinedParticipantsAsync(author.Id, comment.EventId);
            var message = $"Neuer Kommentar von '{comment.Author.Realname}' für Event '{comment.Event.Name}' ({comment.Event.StartEndTimeAsString}): {comment.Text}";

            await InsertNotification(
                comment.Event,
                recipients,
                NotificationType.Commented,
                message,
                author);
        }

        private async Task<List<Diver>> GetAllTauchboldeButDeclinedParticipantsAsync(Guid currentDiverId, Guid eventId)
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
            Event relatedEvent,
            ICollection<Diver> recipients,
            NotificationType notificationType,
            string message,
            Diver currentDiver = null)
        {
            foreach (var recipient in recipients)
            {
                if (
                    currentDiver == null ||
                    currentDiver.SendOwnNoticiations ||
                    currentDiver.Id != recipient.Id)
                {
                    var notification = new Notification
                    {
                        Id = Guid.NewGuid(),
                        Event = relatedEvent,
                        OccuredAt = DateTime.Now,
                        Recipient = recipient,
                        Type = notificationType,
                        Message = message,
                    };

                    await notificationRepository.InsertAsync(notification);

                    TrackEvent("NOTIFICATION-INSERT", notification);
                }
            }
        }

        private void TrackEvent(string name, Notification notification)
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