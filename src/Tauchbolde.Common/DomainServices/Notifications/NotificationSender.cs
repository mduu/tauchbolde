using System;
using System.Threading.Tasks;
using Tauchbolde.Common.Repositories;

namespace Tauchbolde.Common.DomainServices.Notifications
{
    public class NotificationSender : INotificationSender
    {
        public async Task Send(
            INotificationRepository notificationRepository,
            IApplicationUserRepository userRepository,
            INotificationFormatter notificationFormatter,
            INotificationSubmitter notificationSubmitter)
        {
            if (notificationRepository == null) throw new ArgumentNullException(nameof(notificationRepository));
            if (userRepository == null) throw new ArgumentNullException(nameof(userRepository));

            // Check
            var pendingNotifications = await notificationRepository.GetPendingNotificationByUser();
            foreach (var pendingNotification in pendingNotifications)
            {
                var recipient = pendingNotification.Key;
                if (!recipient.AdditionalUserInfos.LastNotificationCheckAt.HasValue ||
                    recipient.AdditionalUserInfos.LastNotificationCheckAt.Value.AddHours(
                        recipient.AdditionalUserInfos.NotificationIntervalInHours) > DateTime.Now)
                {

                    foreach (var notification in pendingNotification)
                    {
                        notification.CountOfTries++;
                    }

                    // Format
                    var content = notificationFormatter.Format(recipient, pendingNotification);
                    if (!string.IsNullOrWhiteSpace(content))
                    {
                        try
                        {
                            // Submit
                            await notificationSubmitter.SubmitAsync(recipient, content);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            throw;
                        }
                    }

                    foreach (var notification in pendingNotification)
                    {
                        notification.CountOfTries++;
                    }

                    recipient.AdditionalUserInfos.LastNotificationCheckAt = DateTime.Now;
                }
            }
        }
    }
}