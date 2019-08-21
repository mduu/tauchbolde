using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.Application.Services.Notifications;
using Tauchbolde.Domain.Events.LogbookEntry;
using Tauchbolde.Domain.Types;

namespace Tauchbolde.Application.Policies.Logbook.LogbookEntryPublished
{
    [UsedImplicitly]
    public class PublishNewLogbookEntryNotificationPolicy : INotificationHandler<LogbookEntryPublishedEvent>
    {
        [NotNull] private readonly IDiverRepository diverRepository;
        [NotNull] private readonly ILogbookEntryRepository logbookEntryRepository;
        [NotNull] private readonly INotificationPublisher notificationPublisher;

        public PublishNewLogbookEntryNotificationPolicy(
            [NotNull] IDiverRepository diverRepository,
            [NotNull] ILogbookEntryRepository logbookEntryRepository,
            [NotNull] INotificationPublisher notificationPublisher)
        {
            this.diverRepository = diverRepository ?? throw new ArgumentNullException(nameof(diverRepository));
            this.logbookEntryRepository = logbookEntryRepository ?? throw new ArgumentNullException(nameof(logbookEntryRepository));
            this.notificationPublisher = notificationPublisher ?? throw new ArgumentNullException(nameof(notificationPublisher));
        }
        
        public async Task Handle([NotNull] LogbookEntryPublishedEvent notification, CancellationToken cancellationToken)
        {
            if (notification == null) throw new ArgumentNullException(nameof(notification));

            var recipients = await diverRepository.GetAllTauchboldeUsersAsync();
            var logbookEntry = await logbookEntryRepository.FindByIdAsync(notification.LogbookEntryId);
            var author = await diverRepository.FindByIdAsync(logbookEntry.OriginalAuthorId);
            var message = $"Neuer Logbucheintrag '{logbookEntry.Title}' von {author.Realname}.";

            await notificationPublisher.PublishAsync(
                NotificationType.NewLogbookEntry,
                message,
                recipients,
                relatedLogbookEntryId: notification.LogbookEntryId);
        }
    }
}