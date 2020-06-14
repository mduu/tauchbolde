using Tauchbolde.Domain.Types;

namespace Tauchbolde.Application.Services.Notifications
{
    /// <summary>
    /// Provides access to additional infos for <see cref="NotificationType"/>.
    /// </summary>
    public interface INotificationTypeInfos
    {
        string GetCaption(NotificationType notificationType);
        string GetIconBase64(NotificationType notificationType);
    }
}