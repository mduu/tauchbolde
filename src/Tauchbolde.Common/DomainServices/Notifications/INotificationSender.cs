using System.Threading.Tasks;

namespace Tauchbolde.Common.DomainServices.Notifications
{
    public interface INotificationSender
    {
        Task SendAsync(INotificationFormatter notificationFormatter,
            INotificationSubmitter notificationSubmitter);
    }
}