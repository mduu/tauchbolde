using System.Threading.Tasks;
using Tauchbolde.Common.Repositories;

namespace Tauchbolde.Common.DomainServices.Notifications
{
    public interface INotificationSender
    {
        Task Send(INotificationRepository notificationRepository, INotificationFormatter notificationFormatter, INotificationSubmitter notificationSubmitter);
    }
}
