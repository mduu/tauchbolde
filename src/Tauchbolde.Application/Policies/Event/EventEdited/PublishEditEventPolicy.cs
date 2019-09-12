using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using Microsoft.Extensions.Logging;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.Application.Services.Notifications;
using Tauchbolde.Domain.Events.Event;
using Tauchbolde.Domain.Types;
using Tauchbolde.SharedKernel.Extensions;

namespace Tauchbolde.Application.Policies.Event.EventEdited
{
    [UsedImplicitly]
    internal class PublishEditEventPolicy : INotificationHandler<EventEditedEvent>
    {
        [NotNull] private readonly ILogger<PublishEditEventPolicy> logger;
        [NotNull] private readonly INotificationPublisher notificationPublisher;
        [NotNull] private readonly IRecipientsBuilder recipientsBuilder;
        [NotNull] private readonly IEventRepository eventRepository;

        public PublishEditEventPolicy(
            [NotNull] ILogger<PublishEditEventPolicy> logger,
            [NotNull] INotificationPublisher notificationPublisher,
            [NotNull] IRecipientsBuilder recipientsBuilder,
            [NotNull] IEventRepository eventRepository)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.notificationPublisher = notificationPublisher ?? throw new ArgumentNullException(nameof(notificationPublisher));
            this.recipientsBuilder = recipientsBuilder ?? throw new ArgumentNullException(nameof(recipientsBuilder));
            this.eventRepository = eventRepository ?? throw new ArgumentNullException(nameof(eventRepository));
        }

        public async Task Handle([NotNull] EventEditedEvent notification, CancellationToken cancellationToken)
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

            var message = $"Aktivität '{evt.Name}' ({evt.StartTime.FormatTimeRange(evt.EndTime)}) von {evt.Organisator?.Realname ?? ""} geändert";

            await notificationPublisher.PublishAsync(
                NotificationType.EditEvent,
                message,
                recipients,
                relatedEventId: evt.Id);
        }
    }
}