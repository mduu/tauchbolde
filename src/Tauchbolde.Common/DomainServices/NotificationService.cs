using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tauchbolde.Common.Model;
using Tauchbolde.Common.Repositories;

namespace Tauchbolde.Common.DomainServices
{
    public class NotificationService : INotificationService
    {
        public async Task NotifyForNewEventAsync(
            INotificationRepository notificationRepository,
            IApplicationUserRepository userRepository,
            Event newEvent)
        {
            if (notificationRepository == null) throw new ArgumentNullException(nameof(notificationRepository));
            if (userRepository == null) throw new ArgumentNullException(nameof(userRepository));
            if (newEvent == null) throw new ArgumentNullException(nameof(newEvent));

            var recipients = await userRepository.GetAllTauchboldeUsersAsync();
            var message = $"Neue Aktivität '{newEvent.Name}' ({newEvent.StartEndTimeAsString}) erstellt";

            await InsertNotification(notificationRepository, newEvent, recipients, NotificationType.NewEvent, message);
        }

        public async Task NotifyForChangedEventAsync(
            INotificationRepository notificationRepository,
            IApplicationUserRepository userRepository,
            Event changedEvent)
        {
            if (notificationRepository == null) throw new ArgumentNullException(nameof(notificationRepository));
            if (userRepository == null) throw new ArgumentNullException(nameof(userRepository));
            if (changedEvent == null) throw new ArgumentNullException(nameof(changedEvent));

            var recipients = await userRepository.GetAllTauchboldeUsersAsync();
            var message = $"Aktivität geändert '{changedEvent.Name}' ({changedEvent.StartEndTimeAsString})";

            await InsertNotification(notificationRepository, changedEvent, recipients, NotificationType.EditEvent, message);
        }

        public async Task NotifyForCanceledEventAsync(INotificationRepository notificationRepository, IApplicationUserRepository userRepository, IParticipantRepository participantRepository, Event canceledEvent)
        {
            if (notificationRepository == null) throw new ArgumentNullException(nameof(notificationRepository));
            if (userRepository == null) throw new ArgumentNullException(nameof(userRepository));
            if (canceledEvent == null) throw new ArgumentNullException(nameof(canceledEvent));

            // Recipients alle Tauchbolde that did not yet "decline" the Event already
            var declinedParticipants = await participantRepository.GetParticipantsForEventByStatusAsync(participant.EventId, ParticipantStatus.Declined);
            var recipients = (await userRepository.GetAllTauchboldeUsersAsync())
                .Where(u => declinedParticipants.All(p => p.User.Id != u.Id))
                .ToList();

            var message = $"Aktivität '{canceledEvent.Name}' ({canceledEvent.StartEndTimeAsString}) wurde abgesagt.";

            await InsertNotification(notificationRepository, canceledEvent, recipients, NotificationType.CancelEvent, message);
        }

        public async Task NotifyForChangedParticipation(INotificationRepository notificationRepository, IApplicationUserRepository userRepository, IParticipantRepository participantRepository, Participant participant)
        {
            if (notificationRepository == null) throw new ArgumentNullException(nameof(notificationRepository));
            if (userRepository == null) throw new ArgumentNullException(nameof(userRepository));
            if (participant == null) throw new ArgumentNullException(nameof(participant));

            // Recipients alle Tauchbolde that did not yet "decline" the Event already
            var declinedParticipants = await participantRepository.GetParticipantsForEventByStatusAsync(participant.EventId, ParticipantStatus.Declined);
            var recipients = (await userRepository.GetAllTauchboldeUsersAsync())
                .Where(u => declinedParticipants.All(p => p.User.Id != u.Id))
                .ToList();

            var message = "";
            NotificationType notificationType;
            switch (participant.Status)
            {
                case ParticipantStatus.None:
                    message = $"Teilnahme: {participant.User.UserName} weiss nicht ob Er/Sie an der Aktivität '{participant.Event.Name}' ({participant.Event.StartEndTimeAsString}) teil nimmt.";
                    notificationType = NotificationType.Neutral;
                    break;
                case ParticipantStatus.Accepted:
                    message = $"Teilnahme: {participant.User.UserName} nimmt an der Aktivität '{participant.Event.Name}' ({participant.Event.StartEndTimeAsString}) teil.";
                    notificationType = NotificationType.Accepted;
                    break;
                case ParticipantStatus.Declined:
                    message = $"Teilnahme: {participant.User.UserName} hat für die Aktivität '{participant.Event.Name}' ({participant.Event.StartEndTimeAsString}) abgesagt.";
                    notificationType = NotificationType.Declined;
                    break;
                case ParticipantStatus.Tentative:
                    message = $"Teilnahme: {participant.User.UserName} nimmt eventuell an der Aktivität '{participant.Event.Name}' ({participant.Event.StartEndTimeAsString}) teil.";
                    notificationType = NotificationType.Tentative;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            await InsertNotification(notificationRepository, participant.Event, recipients, notificationType, message);
        }

        public async Task NotifyForEventComment(INotificationRepository notificationRepository, IApplicationUserRepository userRepository, Comment comment)
        {
            throw new NotImplementedException();
        }

        public async Task NotifyForNewPost(INotificationRepository notificationRepository, IApplicationUserRepository userRepository, Post newPost)
        {
            throw new NotImplementedException();
        }

        private static async Task InsertNotification(
            INotificationRepository notificationRepository,
            Event relatedEvent,
            ICollection<ApplicationUser> recipients,
            NotificationType notificationType, string message)
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
