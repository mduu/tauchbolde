using Tauchbolde.Entities;

namespace Tauchbolde.Common.Domain.Notifications
{
    /// <summary>
    /// Provides access to additional infos for <see cref="NotificationType"/>.
    /// </summary>
    public interface INotificationTypeInfos
    {
        string GetCaption(NotificationType notificationType);
    }
}