using JetBrains.Annotations;
using Tauchbolde.Domain.Entities;
using Tauchbolde.Domain.Types;

namespace Tauchbolde.Application.Services.Notifications
{
    public interface INotificationPublisher
    {
        Task PublishAsync(NotificationType notificationType,
            [CanBeNull] string message,
            [CanBeNull] IEnumerable<Diver> recipients,
            [CanBeNull] Diver currentDiver = null,
            [CanBeNull] Guid? relatedEventId = null,
            [CanBeNull] Guid? relatedLogbookEntryId = null);
    }
}