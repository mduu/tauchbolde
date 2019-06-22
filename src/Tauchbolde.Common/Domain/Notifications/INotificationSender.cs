using System.Threading.Tasks;

namespace Tauchbolde.Common.Domain.Notifications
{
    public interface INotificationSender
    {
        Task SendAsync(INotificationFormatter notificationFormatter,
            INotificationSubmitter notificationSubmitter);
    }
}