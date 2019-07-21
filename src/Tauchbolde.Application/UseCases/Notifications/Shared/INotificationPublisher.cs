using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Tauchbolde.Domain.Entities;
using Tauchbolde.Domain.Types;

namespace Tauchbolde.Application.UseCases.Notifications.Shared
{
    public interface INotificationPublisher
    {
        Task PublishAsync(NotificationType notificationType,
            [CanBeNull] string message,
            [CanBeNull] IEnumerable<Diver> recipients,
            [CanBeNull] Event relatedEvent,
            [CanBeNull] Diver currentDiver = null,
            [CanBeNull] LogbookEntry relatedLogbookEntry = null);
    }
}