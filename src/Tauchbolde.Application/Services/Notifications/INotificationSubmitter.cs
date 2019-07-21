using System.Threading.Tasks;
using Tauchbolde.Domain.Entities;

namespace Tauchbolde.Application.Services.Notifications
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
        Task SubmitAsync(Diver recipient, string content);
    }
}
