using Tauchbolde.Common.Model;

namespace Tauchbolde.Common.DomainServices.Notifications
{
    /// <summary>
    /// Provides access to additional infos for <see cref="NotificationType"/>.
    /// </summary>
    public interface INotificationTypeInfos
    {
        string GetCaption(NotificationType notificationType);
    }
}