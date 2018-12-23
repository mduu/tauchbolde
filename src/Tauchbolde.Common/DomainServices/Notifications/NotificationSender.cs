using System;
using System.Threading.Tasks;
using Tauchbolde.Common.Repositories;

namespace Tauchbolde.Common.DomainServices.Notifications
{
    public class NotificationSender : INotificationSender
    {
        public async Task SendAsync(
            INotificationRepository notificationRepository,
            INotificationFormatter notificationFormatter,
            INotificationSubmitter notificationSubmitter)
        {
            if (notificationRepository == null) throw new ArgumentNullException(nameof(notificationRepository));

            // Check
            var pendingNotifications = await notificationRepository.GetPendingNotificationByUserAsync();
            foreach (var pendingNotificationsForRecipient in pendingNotifications)
            {
                var recipient = pendingNotificationsForRecipient.Key;
                if (!recipient.LastNotificationCheckAt.HasValue ||
                    recipient.LastNotificationCheckAt.Value.AddHours(
                        recipient.NotificationIntervalInHours) < DateTime.Now)
                {
                    // Format
                    var content = notificationFormatter.Format(recipient, pendingNotificationsForRecipient);
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
                        finally
                        {
                            foreach (var notification in pendingNotificationsForRecipient)
                            {
                                notification.CountOfTries++;
                            }
                        }
                    }

                    recipient.LastNotificationCheckAt = DateTime.Now;
                }
            }
        }
    }
}