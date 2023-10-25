using JetBrains.Annotations;
using MediatR;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.Application.Services.Notifications;
using Tauchbolde.Domain.Events.Event;
using Tauchbolde.Domain.Types;
using Tauchbolde.SharedKernel.Extensions;
using INotificationPublisher = Tauchbolde.Application.Services.Notifications.INotificationPublisher;

namespace Tauchbolde.Application.Policies.Event.CommentEdited
{
    [UsedImplicitly]
    internal class PublishEditEventCommentNotificationPolicy : INotificationHandler<CommentEditedEvent>
    {
        [NotNull] private readonly ICommentRepository commentRepository;
        [NotNull] private readonly IDiverRepository diverRepository;
        [NotNull] private readonly IEventRepository eventRepository;
        [NotNull] private readonly INotificationPublisher notificationPublisher;
        [NotNull] private readonly IRecipientsBuilder recipientsBuilder;

        public PublishEditEventCommentNotificationPolicy(
            [NotNull] ICommentRepository commentRepository,
            [NotNull] IDiverRepository diverRepository,
            [NotNull] IEventRepository eventRepository,
            [NotNull] INotificationPublisher notificationPublisher,
            [NotNull] IRecipientsBuilder recipientsBuilder)
        {
            this.commentRepository = commentRepository ?? throw new ArgumentNullException(nameof(commentRepository));
            this.diverRepository = diverRepository ?? throw new ArgumentNullException(nameof(diverRepository));
            this.eventRepository = eventRepository ?? throw new ArgumentNullException(nameof(eventRepository));
            this.notificationPublisher = notificationPublisher ?? throw new ArgumentNullException(nameof(notificationPublisher));
            this.recipientsBuilder = recipientsBuilder ?? throw new ArgumentNullException(nameof(recipientsBuilder));
        }

        public async Task Handle(CommentEditedEvent notification, CancellationToken cancellationToken)
        {
            var comment = await commentRepository.FindByIdAsync(notification.CommentId);
            var author = await diverRepository.FindByIdAsync(notification.AuthorId);
            var evt = await eventRepository.FindByIdAsync(notification.EventId);
            var recipients = await recipientsBuilder.GetAllTauchboldeButDeclinedParticipantsAsync(
                author.Id, 
                notification.EventId);
            var startEndTimeAsString = evt.StartTime.FormatTimeRange(evt.EndTime);
            var message =
                $"Neuer Kommentar von '{author.Realname}' für Event '{evt.Name}' ({startEndTimeAsString}): {comment?.Text}";

            await notificationPublisher.PublishAsync(
                NotificationType.Commented,
                message,
                recipients,
                relatedEventId: notification.EventId);
        }
    }
}