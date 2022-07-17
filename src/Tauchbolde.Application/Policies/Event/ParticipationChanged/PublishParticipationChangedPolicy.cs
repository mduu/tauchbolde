using JetBrains.Annotations;
using MediatR;
using Microsoft.Extensions.Logging;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.Application.Services.Notifications;
using Tauchbolde.Domain.Entities;
using Tauchbolde.Domain.Events.Event;
using Tauchbolde.Domain.Types;

namespace Tauchbolde.Application.Policies.Event.ParticipationChanged
{
    [UsedImplicitly]
    internal class PublishParticipationChangedPolicy : INotificationHandler<ParticipationChangedEvent>
    {
        [NotNull] private readonly ILogger logger;
        [NotNull] private readonly IDiverRepository diverRepository;
        [NotNull] private readonly IEventRepository eventRepository;
        [NotNull] private readonly IParticipantRepository participantRepository;
        [NotNull] private readonly INotificationPublisher notificationPublisher;
        [NotNull] private readonly IRecipientsBuilder recipientsBuilder;

        [NotNull] private readonly IDictionary<ParticipantStatus, NotificationType> mapParticipationStatusToNotificationType =
            new Dictionary<ParticipantStatus, NotificationType>
            {
                {ParticipantStatus.None, NotificationType.Neutral},
                {ParticipantStatus.Accepted, NotificationType.Accepted},
                {ParticipantStatus.Declined, NotificationType.Declined},
                {ParticipantStatus.Tentative, NotificationType.Tentative}
            };

        public PublishParticipationChangedPolicy(
            [NotNull] ILogger<PublishParticipationChangedPolicy> logger,
            [NotNull] IDiverRepository diverRepository,
            [NotNull] IEventRepository eventRepository,
            [NotNull] IParticipantRepository participantRepository,
            [NotNull] INotificationPublisher notificationPublisher,
            [NotNull] IRecipientsBuilder recipientsBuilder)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.diverRepository = diverRepository ?? throw new ArgumentNullException(nameof(diverRepository));
            this.eventRepository = eventRepository ?? throw new ArgumentNullException(nameof(eventRepository));
            this.participantRepository = participantRepository ?? throw new ArgumentNullException(nameof(participantRepository));
            this.notificationPublisher = notificationPublisher ?? throw new ArgumentNullException(nameof(notificationPublisher));
            this.recipientsBuilder = recipientsBuilder ?? throw new ArgumentNullException(nameof(recipientsBuilder));
        }

        public async Task Handle([NotNull] ParticipationChangedEvent notification, CancellationToken cancellationToken)
        {
            if (notification == null) throw new ArgumentNullException(nameof(notification));

            var participant = await GetParticipantAsync(notification);
            var eventName = await GetEventNameAsync(participant.EventId);
            var diverRealName = await GetDiverRealNameAsync(participant.ParticipatingDiverId);
            var noteText = !string.IsNullOrWhiteSpace(participant.Note)
                ? $"Notiz: {participant.Note}"
                : "";

            var messageBuilder = new Dictionary<ParticipantStatus, Func<string>>
            {
                {ParticipantStatus.None, () => $"{diverRealName} weiss nicht ob Er/Sie an '{eventName}' teil nimmt. {noteText}".Trim()},
                {ParticipantStatus.Accepted, () => $"{diverRealName} nimmt an '{eventName}' teil. {noteText}".Trim()},
                {ParticipantStatus.Declined, () => $"{diverRealName} hat für '{eventName}' abgesagt. {noteText}".Trim()},
                {ParticipantStatus.Tentative, () => $"{diverRealName} nimmt eventuell an '{eventName}' teil. {noteText}".Trim()}
            };

            var recipients = await recipientsBuilder.GetAllTauchboldeButDeclinedParticipantsAsync(
                participant.ParticipatingDiverId,
                participant.EventId);

            await notificationPublisher.PublishAsync(
                mapParticipationStatusToNotificationType[participant.Status],
                messageBuilder[participant.Status](),
                recipients,
                relatedEventId: participant.EventId);
        }

        private async Task<string> GetDiverRealNameAsync(Guid diverId)
        {
            var result = await diverRepository.FindByIdAsync(diverId);
            if (result == null)
            {
                logger.LogError("Diver with ID {id} not found!", diverId);
                throw new InvalidOperationException("Diver not found!");
            }

            return result.Realname ?? "Unbekannt";
        }

        private async Task<Participant> GetParticipantAsync(ParticipationChangedEvent notification)
        {
            var result = await participantRepository.GetParticipantByIdAsync(notification.ParticipationId);
            if (result == null)
            {
                logger.LogError("Participant with ID {id} not found.", notification.ParticipationId);
                throw new InvalidOperationException("Participant not found!");
            }
            
            return result;
        }
        
        private async Task<string> GetEventNameAsync(Guid eventId)
        {
            var result = await eventRepository.FindByIdAsync(eventId);
            if (result == null)
            {
                logger.LogError("Event with ID {id} not found!", eventId);
                throw new InvalidOperationException("Event not found!");
            }
            
            return result.Name;
        }
    }
}