using System.Threading.Tasks;
using Tauchbolde.Common.DataAccess;

namespace Tauchbolde.Common.DomainServices.Notifications
{
    public interface INotificationSender
    {
        Task SendAsync(
            INotificationRepository notificationRepository,
            INotificationFormatter notificationFormatter,
            INotificationSubmitter notificationSubmitter);
    }
}
