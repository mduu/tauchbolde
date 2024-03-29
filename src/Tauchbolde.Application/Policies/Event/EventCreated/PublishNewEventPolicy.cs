using JetBrains.Annotations;
using MediatR;
using Microsoft.Extensions.Logging;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.Application.Services.Notifications;
using Tauchbolde.Domain.Events.Event;
using Tauchbolde.Domain.Types;
using Tauchbolde.SharedKernel.Extensions;
using INotificationPublisher = Tauchbolde.Application.Services.Notifications.INotificationPublisher;

namespace Tauchbolde.Application.Policies.Event.EventCreated
{
    [UsedImplicitly]
    internal class PublishNewEventPolicy : INotificationHandler<EventCreatedEvent>
    {
        [NotNull] private readonly ILogger<PublishNewEventPolicy> logger;
        [NotNull] private readonly INotificationPublisher notificationPublisher;
        [NotNull] private readonly IRecipientsBuilder recipientsBuilder;
        [NotNull] private readonly IEventRepository eventRepository;

        public PublishNewEventPolicy(
            [NotNull] ILogger<PublishNewEventPolicy> logger,
            [NotNull] INotificationPublisher notificationPublisher,
            [NotNull] IRecipientsBuilder recipientsBuilder,
            [NotNull] IEventRepository eventRepository)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.notificationPublisher = notificationPublisher ?? throw new ArgumentNullException(nameof(notificationPublisher));
            this.recipientsBuilder = recipientsBuilder ?? throw new ArgumentNullException(nameof(recipientsBuilder));
            this.eventRepository = eventRepository ?? throw new ArgumentNullException(nameof(eventRepository));
        }

        public async Task Handle([NotNull] EventCreatedEvent notification, CancellationToken cancellationToken)
        {
            if (notification == null) throw new ArgumentNullException(nameof(notification));

            var evt = await eventRepository.FindByIdAsync(notification.EventId);
            if (evt == null)
            {
                logger.LogError("Event [{id}] not found!", notification.EventId);
                return;
            }

            var recipients = await recipientsBuilder.GetAllTauchboldeButDeclinedParticipantsAsync(
                evt.OrganisatorId,
                evt.Id);

            var message = $"Neue Aktivität '{evt.Name}' ({evt.StartTime.FormatTimeRange(evt.EndTime)}) von {evt.Organisator.Realname} erstellt.";

            await notificationPublisher.PublishAsync(
                NotificationType.NewEvent,
                message,
                recipients,
                relatedEventId: evt.Id);
        }
    }
}