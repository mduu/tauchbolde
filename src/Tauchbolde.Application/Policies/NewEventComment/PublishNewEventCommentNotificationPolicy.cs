using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.Application.Services.Notifications;
using Tauchbolde.Domain.Events.Event;
using Tauchbolde.Domain.Types;

namespace Tauchbolde.Application.Policies.NewEventComment
{
    [UsedImplicitly]
    internal class PublishNewEventCommentNotificationPolicy : INotificationHandler<NewEventCommentEvent>
    {
        [NotNull] private readonly IDiverRepository diverRepository;
        [NotNull] private readonly IEventRepository eventRepository;
        [NotNull] private readonly INotificationPublisher notificationPublisher;
        [NotNull] private readonly IRecipientsBuilder recipientsBuilder;

        public PublishNewEventCommentNotificationPolicy(
            [NotNull] IDiverRepository diverRepository,
            [NotNull] IEventRepository eventRepository,
            [NotNull] INotificationPublisher notificationPublisher,
            [NotNull] IRecipientsBuilder recipientsBuilder)
        {
            this.diverRepository = diverRepository ?? throw new ArgumentNullException(nameof(diverRepository));
            this.eventRepository = eventRepository ?? throw new ArgumentNullException(nameof(eventRepository));
            this.notificationPublisher = notificationPublisher ?? throw new ArgumentNullException(nameof(notificationPublisher));
            this.recipientsBuilder = recipientsBuilder ?? throw new ArgumentNullException(nameof(recipientsBuilder));
        }
        
        public async Task Handle([NotNull] NewEventCommentEvent notification, CancellationToken cancellationToken)
        {
            if (notification == null) throw new ArgumentNullException(nameof(notification));

            var author = await diverRepository.FindByIdAsync(notification.AuthorId);
            var evt = await eventRepository.FindByIdAsync(notification.EventId);
            var recipients = await recipientsBuilder.GetAllTauchboldeButDeclinedParticipantsAsync(
                author.Id, 
                notification.EventId);
            var message = $"Neuer Kommentar von '{author.Realname}' für Event '{evt.Name}' ({evt.StartEndTimeAsString}): {notification.Text}";

            await notificationPublisher.PublishAsync(
                NotificationType.Commented,
                message,
                recipients,
                relatedEventId: notification.EventId);
        }
    }
}