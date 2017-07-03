using System.Threading.Tasks;
using Tauchbolde.Common.Model;

namespace Tauchbolde.Common.DomainServices.Notifications
{
    /// <summary>
    /// Allows to send out notification.
    /// </summary>
    public interface INotificationSubmitter
    {
        /// <summary>
        /// Submits one single Notification asynchronous.
        /// </summary>
        /// <param name="recipient">Recipient to submit the Notification to.</param>
        /// <param name="content">Text content of the notification.</param>
        Task SubmitAsync(ApplicationUser recipient, string content);
    }
}
