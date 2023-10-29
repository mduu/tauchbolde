using Tauchbolde.Domain.Entities;

namespace Tauchbolde.Application.Services.Notifications
{
    /// <summary>
    /// Service that allows formatting Notifications.
    /// </summary>
    public interface INotificationFormatter
    {
        /// <summary>
        /// Returns a formated text of the list of notifications.
        /// </summary>
        /// <param name="recipient">The recipient to address.</param>
        /// <param name="notifications">The notifications to format.</param>
        /// <returns>A formated text of the list of notifications.</returns>
        Task<string> FormatAsync(Diver recipient, IEnumerable<Notification> notifications);
    }
}
