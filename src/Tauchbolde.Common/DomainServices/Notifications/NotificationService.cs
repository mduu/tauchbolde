using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tauchbolde.Common.Model;
using Tauchbolde.Common.Repositories;

namespace Tauchbolde.Common.DomainServices.Notifications
{
    /// <summary>
    /// The notification service allows the app to register notifications to be send somewhen later on.
    /// </summary>
    public class NotificationService : INotificationService
    {
        /// <inheritdoc />
        public async Task NotifyForNewEventAsync(
            INotificationRepository notificationRepository,
            IApplicationUserRepository userRepository,
            Event newEvent)
        {
            if (notificationRepository == null) throw new ArgumentNullException(nameof(notificationRepository));
            if (userRepository == null) throw new ArgumentNullException(nameof(userRepository));
            if (newEvent == null) throw new ArgumentNullException(nameof(newEvent));

            var recipients = await userRepository.GetAllTauchboldeUsersAsync();
            var message = $"Neue Aktivität '{newEvent.Name}' ({newEvent.StartEndTimeAsString}) von {newEvent.Organisator.Realname} erstellt";

            await InsertNotification(notificationRepository, newEvent, recipients, NotificationType.NewEvent, message);
        }

        /// <inheritdoc />
        public async Task NotifyForChangedEventAsync(
            INotificationRepository notificationRepository,
            IApplicationUserRepository userRepository,
            Event changedEvent)
        {
            if (notificationRepository == null) throw new ArgumentNullException(nameof(notificationRepository));
            if (userRepository == null) throw new ArgumentNullException(nameof(userRepository));
            if (changedEvent == null) throw new ArgumentNullException(nameof(changedEvent));

            var recipients = await userRepository.GetAllTauchboldeUsersAsync();
            var message = $"Aktivität geändert '{changedEvent.Name}' ({changedEvent.StartEndTimeAsString}) von {changedEvent.Organisator.Realname}";

            await InsertNotification(notificationRepository, changedEvent, recipients, NotificationType.EditEvent, message);
        }

        /// <inheritdoc />
        public async Task NotifyForCanceledEventAsync(INotificationRepository notificationRepository, IApplicationUserRepository userRepository, IParticipantRepository participantRepository, Event canceledEvent)
        {
            if (notificationRepository == null) throw new ArgumentNullException(nameof(notificationRepository));
            if (userRepository == null) throw new ArgumentNullException(nameof(userRepository));
            if (canceledEvent == null) throw new ArgumentNullException(nameof(canceledEvent));

            // Recipients alle Tauchbolde that did not yet "decline" the Event already
            var declinedParticipants = await participantRepository.GetParticipantsForEventByStatusAsync(canceledEvent.Id, ParticipantStatus.Declined);
            var recipients = (await userRepository.GetAllTauchboldeUsersAsync())
                .Where(u => declinedParticipants.All(p => p.ParticipatingDiver.Id != u.Id))
                .ToList();

            var message = $"Aktivität '{canceledEvent.Name}' ({canceledEvent.StartEndTimeAsString}) wurde abgesagt von {canceledEvent.Organisator.Realname}.";

            await InsertNotification(notificationRepository, canceledEvent, recipients, NotificationType.CancelEvent, message);
        }

        /// <inheritdoc />
        public async Task NotifyForChangedParticipation(INotificationRepository notificationRepository, IApplicationUserRepository userRepository, IParticipantRepository participantRepository, Participant participant)
        {
            if (notificationRepository == null) throw new ArgumentNullException(nameof(notificationRepository));
            if (userRepository == null) throw new ArgumentNullException(nameof(userRepository));
            if (participant == null) throw new ArgumentNullException(nameof(participant));

            var recipients = await GetAllTauchboldeButDeclinedParticipantsAsync(userRepository, participantRepository, participant.EventId);

            var message = "";
            NotificationType notificationType;
            switch (participant.Status)
            {
                case ParticipantStatus.None:
                    message = $"Teilnahme: {participant.ParticipatingDiver.Realname} weiss nicht ob Er/Sie an der Aktivität '{participant.Event.Name}' ({participant.Event.StartEndTimeAsString}) teil nimmt.";
                    notificationType = NotificationType.Neutral;
                    break;
                case ParticipantStatus.Accepted:
                    message = $"Teilnahme: {participant.ParticipatingDiver.Realname} nimmt an der Aktivität '{participant.Event.Name}' ({participant.Event.StartEndTimeAsString}) teil.";
                    notificationType = NotificationType.Accepted;
                    break;
                case ParticipantStatus.Declined:
                    message = $"Teilnahme: {participant.ParticipatingDiver.Realname} hat für die Aktivität '{participant.Event.Name}' ({participant.Event.StartEndTimeAsString}) abgesagt.";
                    notificationType = NotificationType.Declined;
                    break;
                case ParticipantStatus.Tentative:
                    message = $"Teilnahme: {participant.ParticipatingDiver.Realname} nimmt eventuell an der Aktivität '{participant.Event.Name}' ({participant.Event.StartEndTimeAsString}) teil.";
                    notificationType = NotificationType.Tentative;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            await InsertNotification(notificationRepository, participant.Event, recipients, notificationType, message);
        }

        /// <inheritdoc />
        public async Task NotifyForEventComment(INotificationRepository notificationRepository, IApplicationUserRepository userRepository, IParticipantRepository participantRepository, Comment comment)
        {
            if (notificationRepository == null) throw new ArgumentNullException(nameof(notificationRepository));
            if (userRepository == null) throw new ArgumentNullException(nameof(userRepository));
            if (comment == null) throw new ArgumentNullException(nameof(comment));

            var recipients = await GetAllTauchboldeButDeclinedParticipantsAsync(userRepository, participantRepository, comment.EventId);
            var message = $"Neuer Kommentar von '{comment.Author.Realname}' für Event '{comment.Event.Name}' ({comment.Event.StartEndTimeAsString}): {comment.Text}";

            await InsertNotification(notificationRepository, comment.Event, recipients, NotificationType.Commented, message);
        }

        /// <inheritdoc />
        public async Task NotifyForNewPost(INotificationRepository notificationRepository, IApplicationUserRepository userRepository, Post newPost)
        {
            if (notificationRepository == null) throw new ArgumentNullException(nameof(notificationRepository));
            if (userRepository == null) throw new ArgumentNullException(nameof(userRepository));
            if (newPost == null) throw new ArgumentNullException(nameof(newPost));

            var recipients = await userRepository.GetAllTauchboldeUsersAsync();
            var message = $"Neuer Beitrag von {newPost.Author.Realname} veröffentlicht: {newPost.Title}";

            await InsertNotification(notificationRepository, null, recipients, NotificationType.NewPost, message);
        }

        private async Task<List<Diver>> GetAllTauchboldeButDeclinedParticipantsAsync(
            IApplicationUserRepository userRepository,
            IParticipantRepository participantRepository, Guid eventId)
        {
            var declinedParticipants = await participantRepository.GetParticipantsForEventByStatusAsync(eventId, ParticipantStatus.Declined);

            var result = (await userRepository.GetAllTauchboldeUsersAsync())
                .Where(u => declinedParticipants.All(p => p.ParticipatingDiver.Id != u.Id))
                .ToList();

            return result;
        }

        private static async Task InsertNotification(
            INotificationRepository notificationRepository,
            Event relatedEvent,
            ICollection<Diver> recipients,
            NotificationType notificationType,
            string message)
        {
            foreach (var recipient in recipients)
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
            }
        }
    }
}